using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaMetas")]
    public class Meta : PropostaNode
    {
        public string Objetivo { get; set; }
        public short Meses { get; set; }
    }
}