using TaesaCore.Models;


namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaRiscoDto : BaseEntity
    {
        public string Item { get; set; }
        public string Classificacao { get; set; }
        public string Justificativa { get; set; }
        public string Probabilidade { get; set; }
    }
}