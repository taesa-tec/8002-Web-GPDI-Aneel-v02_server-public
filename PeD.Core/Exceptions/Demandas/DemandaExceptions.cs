namespace PeD.Core.Exceptions.Demandas
{
    [System.Serializable]
    public class DemandaException : PeDException
    {
        public DemandaException() { }
        public DemandaException(string message) : base(message) { }
        public DemandaException(string message, System.Exception inner) : base(message, inner) { }
        protected DemandaException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}