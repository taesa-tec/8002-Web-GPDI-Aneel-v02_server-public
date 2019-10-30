using System;
using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Models.Demandas.Forms
{

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

        public string ItemTitle { get; set; }

        public string FieldType
        {
            get
            {
                return Enum.GetName(typeof(Type), this._FieldType);
            }
        }
        public bool IsArray { get; set; }

        public Dictionary<string, string> Options { get; set; }
        public int Order { get; set; }
        public string Placeholder { get; set; }

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
   
    public class SimpleListField : FieldList
    {
        public SimpleListField(string Key, string Title) : base(Key, Title, Type.Text) { }
    }
    public class RichTextField : Field
    {
        public RichTextField(string Key, string Title) : base(Key, Title, Type.RichText) { }
    }
    public class RichTextListField : FieldList
    {
        public RichTextListField(string Key, string Title) : base(Key, Title, Type.RichText) { }
    }
    public class FieldList : Field
    {
        public bool HasFixedSize { get; set; }
        public FieldList(string Key, string Title, Type FieldType = Type.Empty, bool HasFixedSize = true) : base(Key, Title, FieldType)
        {
            Children = new List<Field>();
            this.HasFixedSize = HasFixedSize;
        }
        public List<Field> Children { get; set; }
        public Field Add(Field field)
        {
            this.Children.Add(field);
            return field;
        }
        public Field AddText(string Key, string Title, string Placeholder = "")
        {
            var field = Field.Text(Key, Title);
            field.Placeholder = Placeholder;
            this.Children.Add(field);
            return field;
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
            this.Children.Add(field);
            return field;
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
            this.Children.Add(field);
            return field;
        }
        public FieldList AddFieldList(string Key, string Title, Type Type = Type.Empty, bool HasFixedSize = true)
        {
            var field = new FieldList(Key, Title, Type, HasFixedSize);
            this.Children.Add(field);
            return field;
        }
        public FieldList AddFieldList(string Key, string Title, bool HasFixedSize = true)
        {
            return AddFieldList(Key, Title, Type.Empty, HasFixedSize);
        }
        public FieldList AddFieldList(string Key, string Title)
        {
            return AddFieldList(Key, Title, Type.Empty);
        }
    }

}