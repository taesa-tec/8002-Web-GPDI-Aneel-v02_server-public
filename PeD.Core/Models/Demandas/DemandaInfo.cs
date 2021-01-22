using PeD.Core.Models.Captacoes;

namespace PeD.Core.Models.Demandas
{
    public class DemandaInfo
    {
        public int Id { get; set; }
        public int CaptacaoId { get; set; }
        public string Form { get; set; }
        public int Revisao { get; set; }
    }
}