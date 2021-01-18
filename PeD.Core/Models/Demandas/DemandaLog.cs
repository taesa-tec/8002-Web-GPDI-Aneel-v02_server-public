namespace PeD.Core.Models.Demandas
{

    public class DemandaLog : Log
    {
        public int DemandaId { get; set; }

        public Demanda Demanda { get; set; }
    }
}