using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRiscos")]
    public class Risco : PropostaNode
    {
        public string Item { get; set; }
        public string Classificacao { get; set; }
        public string Justificativa { get; set; }
        public string Probabilidade { get; set; }
    }
}