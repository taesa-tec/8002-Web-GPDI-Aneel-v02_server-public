using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class Apoio : ProjetoNode
    {
        public enum TipoEstrutura
        {
            LNS,
            LES,
            LNP,
            LEP,
            LNE,
            LEE
        }

        public TipoEstrutura Tipo { get; set; }

        public string CnpjReceptora { get; set; }
        [MaxLength(100)] public string Laboratorio { get; set; }
        [MaxLength(50)] public string LaboratorioArea { get; set; }
        [MaxLength(300)] public string MateriaisEquipamentos { get; set; }
    }
}