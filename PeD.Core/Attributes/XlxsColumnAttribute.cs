using System;

namespace PeD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class XlxsColumnAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public Type Type { get; set; }

        public XlxsColumnAttribute(string name = null, int order = int.MaxValue)
        {
            Name = name;
            Order = order;
        }

        public XlxsColumnAttribute()
        {
        }

        public XlxsColumnAttribute(int order)
        {
            Order = order;
        }
    }
}