using System;
using System.Runtime.Serialization;

namespace PeD.Core.Exceptions
{
    public class PeDException : Exception
    {
        public PeDException()
        {
        }

        protected PeDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PeDException(string message) : base(message)
        {
        }

        public PeDException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}