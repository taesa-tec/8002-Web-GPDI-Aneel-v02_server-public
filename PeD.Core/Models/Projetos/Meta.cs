using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoMetas")]
    public class Meta : ProjetoNode
    {
        public string Objetivo { get; set; }
        public short Meses { get; set; }
    }
}