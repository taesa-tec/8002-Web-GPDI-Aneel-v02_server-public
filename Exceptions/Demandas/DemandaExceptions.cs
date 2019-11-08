using System;
using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Exceptions.Demandas
{
    public class DemandaException : Exception
    {
        public DemandaException(string message) : base(message)
        {

        }
    }
}