using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class Socioambiental : ProjetoNode
    {
        public enum TipoIndicador
        {
            ISA1,
            ISA2,
            ISA3,
            ISA4
        }

        public TipoIndicador Tipo { get; set; }
        public bool ResultadoPositivo { get; set; }
        [MaxLength(500)]
        public string DescricaoResultado { get; set; }
    }
}