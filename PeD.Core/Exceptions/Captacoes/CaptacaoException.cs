using System;
using System.Runtime.Serialization;

namespace PeD.Core.Exceptions.Captacoes
{
    public class CaptacaoException : PeDException
    {
        public CaptacaoException()
        {
        }

        protected CaptacaoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CaptacaoException(string? message) : base(message)
        {
        }

        public CaptacaoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}