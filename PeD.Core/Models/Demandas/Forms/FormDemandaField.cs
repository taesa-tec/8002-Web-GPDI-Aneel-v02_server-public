using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PeD.Core.Models.Demandas.Forms
{
    public delegate FieldRendered DataRender(JToken data);

    public delegate void RenderHandler(Field field, JToken data, ref FieldRendered fieldRendered);

    public class FieldRendered
    {
        public string Title;
        public string Value;
        public string Type;
        public List<FieldRendered> Children;

        public FieldRendered(string Title, string Value)
        {
            this.Title = Title;
            this.Value = Value;
            Type = "";
            Children = new List<FieldRendered>();
        }

        public HtmlNode ToHtml()
        {
            var item = HtmlNode.CreateNode("<div></div>");
            var title = HtmlNode.CreateNode($"<div>{Title}</div>");
            var value = HtmlNode.CreateNode($"<div>{Value}</div>");

            item.AddClass("field-item");
            item.AddClass(Type.ToLower());
            title.AddClass("field-item-title");
            value.AddClass("field-item-value");

            item.AppendChild(title);
            item.AppendChild(value);

            if (Children.Count > 0)
            {
                var children = HtmlNode.CreateNode("<div></div>");
                children.AddClass("field-item-children");
                foreach (var field in Children)
                {
                    children.AppendChild(field.ToHtml());
                }

                item.AppendChild(children);
            }

            return item;
        }
    }

    public class Field
    {
        public enum Type
        {
            Empty,
            Form,
            Text,
            Date,
            Options,
            RichText,
            Temas
        }

        public readonly string Title;
        public readonly string Key;
        protected readonly Type _FieldType;
        public bool IsArray { get; set; }
        public string ItemTitle { get; set; }

        public string FieldType
        {
            get { return Enum.GetName(typeof(Type), _FieldType); }
        }

        public Dictionary<string, string> Options { get; set; }
        public int Order { get; set; }
        public string Placeholder { get; set; }

        [JsonIgnore] public Field Parent { get; set; }
        public DataRender RenderTemaHandler;
        public RenderHandler RenderHandler;

        protected FieldRendered RenderForm(JToken data)
        {
            var fieldRendered = new FieldRendered(Title, "")
            {
                Type = FieldType
            };
            return fieldRendered;
        }

        protected FieldRendered RenderText(JObject data)
        {
            var value = string.Empty;
            if (data.TryGetValue("value", out var token))
            {
                value = token.Value<object>().ToString();
            }

            var fieldRendered = new FieldRendered(IsArray ? ItemTitle : Title, value)
            {
                Type = FieldType
            };
            return fieldRendered;
        }

        public virtual FieldRendered Render(JObject data)
        {
            var fieldRendered = FieldType switch
            {
                "Form" => RenderForm(data),
                _ => RenderText(data)
            };

            fieldRendered.Type = FieldType;
            FieldRenderSanitizer(this, data, ref fieldRendered);
            return fieldRendered;
        }

        public void FieldRenderSanitizer(Field field, JToken data, ref FieldRendered fieldRendered)
        {
            if (RenderHandler != null)
            {
                try
                {
                    RenderHandler(field, data, ref fieldRendered);
                }
                catch (Exception)
                {
                    // ignored
                    // Caso ocorra um erro na renderização de um campo específico (por ter uma estrutura diferente da
                    // anterior, por exemplo), não podemos impendir a renderização dos compos seguintes
                }
            }

            if (Parent != null)
            {
                Parent.FieldRenderSanitizer(field, data, ref fieldRendered);
            }
        }

        public virtual FieldRendered Render(JArray data)
        {
            var fieldRendered = new FieldRendered(Title, "");
            fieldRendered.Children.AddRange(data.Children().Select(child => Render(child as JObject)));
            fieldRendered.Type = FieldType;
            return fieldRendered;
        }

        public Field(string key, string title, Type fieldType = Type.Empty, bool isArray = false)
        {
            Key = key;
            Title = title;
            ItemTitle = title;
            _FieldType = fieldType;
            Placeholder = "";
            IsArray = isArray;
        }


        public static Field Form(string key, string title)
        {
            return new FieldList(key, title, Type.Form);
        }

        public static Field Text(string key, string title)
        {
            return new Field(key, title, Type.Text);
        }

        public static Field RichText(string key, string title)
        {
            return new Field(key, title, Type.RichText);
        }

        public static Field Date(string key, string title)
        {
            return new Field(key, title, Type.Date);
        }

        public static Field OptionsField(string key, string title, Dictionary<string, string> options)
        {
            var field = new Field(key, title, Type.Options)
            {
                Options = options
            };
            return field;
        }
    }

    public class FieldList : Field
    {
        public List<Field> Children { get; set; }
        public bool HasFixedSize { get; set; }

        public FieldList(string key, string title, Type fieldType = Type.Empty, bool hasFixedSize = true) : base(key,
            title, fieldType)
        {
            Children = new List<Field>();
            HasFixedSize = hasFixedSize;
        }

        public override FieldRendered Render(JObject data)
        {
            var fieldRendered = base.Render(data);
            try
            {
                var children = data.Value<JObject>("children");
                if (children != null)
                {
                    Children.ForEach(field =>
                    {
                        if (children.TryGetValue(field.Key, out var child))
                        {
                            if (field.IsArray)
                            {
                                fieldRendered.Children.Add(field.Render(child as JArray));
                            }
                            else
                            {
                                fieldRendered.Children.Add(field.Render(child as JObject));
                            }
                        }
                    });
                }
            }
            catch (Exception)
            {
                // ignored
            }

            fieldRendered.Type = FieldType;
            return fieldRendered;
        }


        #region Add methods

        public Field Add(Field field)
        {
            field.Parent = this;
            Children.Add(field);
            return field;
        }

        public Field AddText(string key, string title, string placeholder = "")
        {
            var field = Text(key, title);
            field.Placeholder = placeholder;
            return Add(field);
        }

        public Field AddTextList(string key, string title, string placeholder = "")
        {
            var field = AddText(key, title, placeholder);
            field.IsArray = true;
            return field;
        }

        public Field AddRichText(string key, string title, string placeholder = "")
        {
            var field = RichText(key, title);
            field.Placeholder = placeholder;
            return Add(field);
        }

        public Field AddRichTextList(string key, string title, string placeholder = "")
        {
            var field = AddRichText(key, title, placeholder);
            field.IsArray = true;
            return field;
        }

        public Field AddEmpty(string key, string title)
        {
            var field = new Field(key, title, Type.Empty);
            return Add(field);
        }

        public FieldList AddFieldList(string key, string title, Type type, bool hasFixedSize = true)
        {
            var field = new FieldList(key, title, type, hasFixedSize);
            return Add(field) as FieldList;
        }

        public FieldList AddFieldList(string key, string title, bool hasFixedSize)
        {
            return AddFieldList(key, title, Type.Empty, hasFixedSize);
        }

        public FieldList AddFieldList(string key, string title)
        {
            return AddFieldList(key, title, Type.Empty);
        }

        #endregion
    }
}