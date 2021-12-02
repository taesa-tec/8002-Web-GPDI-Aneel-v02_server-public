using System;

namespace PeD.Core.Attributes {
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class LoggerAttribute : Attribute {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string name;
        readonly string valueFrom;

        // This is a positional argument
        public LoggerAttribute( string Name = null, string ValueFrom = null ) {
            name = Name;
            valueFrom = ValueFrom;

        }

        public string Name {
            get { return name; }
        }
        public string ValueFrom {
            get {
                return valueFrom;
            }
        }
        public bool hasValueFrom {
            get {
                return valueFrom != null && valueFrom.Trim().Length > 0;
            }
        }


    }
}
