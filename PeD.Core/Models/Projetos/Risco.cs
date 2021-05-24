using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoRiscos")]
    public class Risco : ProjetoNode
    {
        public string Item { get; set; }
        public string Classificacao { get; set; }
        public string Justificativa { get; set; }
        public string Probabilidade { get; set; }
    }
}