using System;

namespace APIGestor.Attributes {
    [System.AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    sealed class LoggerAttribute : Attribute {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string name;
        readonly string valueFrom;

        // This is a positional argument
        public LoggerAttribute( string Name = null, string ValueFrom = null ) {
            this.name = Name;
            this.valueFrom = ValueFrom;

        }

        public string Name {
            get { return name; }
        }
        public string ValueFrom {
            get {
                return this.valueFrom;
            }
        }
        public bool hasValueFrom {
            get {
                return this.valueFrom != null && this.valueFrom.Trim().Length > 0;
            }
        }


    }
}
