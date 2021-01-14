namespace PeD.Core.Models.Demandas
{
    public class DemandaFile : FileUpload
    {
        public int DemandaId { get; set; }
        public Demanda Demanda { get; set; }
    }
}
