using System;
using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Exceptions.Demandas
{
    [System.Serializable]
    public class DemandaException : System.Exception
    {
        public DemandaException() { }
        public DemandaException(string message) : base(message) { }
        public DemandaException(string message, System.Exception inner) : base(message, inner) { }
        protected DemandaException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}