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
            this.Type = "";
            this.Children = new List<FieldRendered>();
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
                foreach (FieldRendered field in Children)
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
            get { return Enum.GetName(typeof(Type), this._FieldType); }
        }

        public Dictionary<string, string> Options { get; set; }
        public int Order { get; set; }
        public string Placeholder { get; set; }

        [JsonIgnore] public Field Parent { get; set; }
        public DataRender RenderTemaHandler;
        public RenderHandler RenderHandler;

        protected FieldRendered RenderForm(JToken data)
        {
            var fieldRendered = new FieldRendered(this.Title, "");
            fieldRendered.Type = this.FieldType;
            return fieldRendered;
        }

        protected FieldRendered RenderText(JObject data)
        {
            string Value = String.Empty;
            try
            {
                if (data.TryGetValue("value", out JToken token))
                {
                    Value = token.Value<object>().ToString();
                }
            }
            catch (System.Exception)
            {
                throw;
            }


            var fieldRendered = new FieldRendered(IsArray ? this.ItemTitle : this.Title, Value);
            fieldRendered.Type = this.FieldType;
            return fieldRendered;
        }

        public virtual FieldRendered Render(JObject data)
        {
            FieldRendered fieldRendered;

            switch (this.FieldType)
            {
                case "Form":
                    fieldRendered = RenderForm(data);
                    break;
                default:
                    fieldRendered = RenderText(data);
                    break;
            }

            fieldRendered.Type = this.FieldType;
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
            var fieldRendered = new FieldRendered(this.Title, "");
            fieldRendered.Children.AddRange(data.Children().Select(child => Render(child as JObject)));
            fieldRendered.Type = this.FieldType;
            return fieldRendered;
        }

        public Field(string Key, string Title, Type FieldType = Type.Empty, bool IsArray = false)
        {
            this.Key = Key;
            this.Title = Title;
            ItemTitle = Title;
            this._FieldType = FieldType;
            this.Placeholder = "";
            this.IsArray = IsArray;
        }


        public static Field Form(string Key, string Title)
        {
            return new FieldList(Key, Title, Type.Form);
        }

        public static Field Text(string Key, string Title)
        {
            return new Field(Key, Title, Type.Text);
        }

        public static Field RichText(string Key, string Title)
        {
            return new Field(Key, Title, Type.RichText);
        }

        public static Field Date(string Key, string Title)
        {
            return new Field(Key, Title, Type.Date);
        }

        public static Field OptionsField(string Key, string Title, Dictionary<string, string> Options)
        {
            var field = new Field(Key, Title, Type.Options);
            field.Options = Options;
            return field;
        }
    }

    public class FieldList : Field
    {
        public List<Field> Children { get; set; }
        public bool HasFixedSize { get; set; }

        public FieldList(string Key, string Title, Type FieldType = Type.Empty, bool HasFixedSize = true) : base(Key,
            Title, FieldType)
        {
            Children = new List<Field>();
            this.HasFixedSize = HasFixedSize;
        }

        public override FieldRendered Render(JObject data)
        {
            var fieldRendered = base.Render(data);
            try
            {
                //FieldRendered fieldRendered = new FieldRendered(this.Title, "");
                var children = data.Value<JObject>("children");
                if (children != null)
                {
                    Children.ForEach(field =>
                    {
                        if (children.TryGetValue(field.Key, out JToken child))
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
            this.Children.Add(field);
            return field;
        }

        public Field AddText(string Key, string Title, string Placeholder = "")
        {
            var field = Field.Text(Key, Title);
            field.Placeholder = Placeholder;
            return Add(field);
        }

        public Field AddTextList(string Key, string Title, string Placeholder = "")
        {
            var field = AddText(Key, Title, Placeholder);
            field.IsArray = true;
            return field;
        }

        public Field AddRichText(string Key, string Title, string Placeholder = "")
        {
            var field = Field.RichText(Key, Title);
            field.Placeholder = Placeholder;
            return Add(field);
        }

        public Field AddRichTextList(string Key, string Title, string Placeholder = "")
        {
            var field = AddRichText(Key, Title, Placeholder);
            field.IsArray = true;
            return field;
        }

        public Field AddEmpty(string Key, string Title)
        {
            var field = new Field(Key, Title, Type.Empty);
            return Add(field);
        }

        public FieldList AddFieldList(string Key, string Title, Type Type = Type.Empty, bool HasFixedSize = true)
        {
            var field = new FieldList(Key, Title, Type, HasFixedSize);
            return Add(field) as FieldList;
        }

        public FieldList AddFieldList(string Key, string Title, bool HasFixedSize = true)
        {
            return AddFieldList(Key, Title, Type.Empty, HasFixedSize);
        }

        public FieldList AddFieldList(string Key, string Title)
        {
            return AddFieldList(Key, Title, Type.Empty);
        }

        #endregion
    }
}