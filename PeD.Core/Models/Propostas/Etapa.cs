using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaEtapas")]
    public class Etapa : PropostaNode
    {
        public string DescricaoAtividades { get; set; }
        
    }
}