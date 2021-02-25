using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class RiscoRequest : BaseEntity
    {
        public string Item { get; set; }
        public string Classificacao { get; set; }
        public string Justificativa { get; set; }
        public string Probabilidade { get; set; }
    }
}