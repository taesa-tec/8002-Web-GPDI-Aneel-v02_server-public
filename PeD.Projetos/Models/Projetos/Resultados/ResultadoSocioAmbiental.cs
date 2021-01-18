using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;

namespace PeD.Projetos.Models.Projetos.Resultados {
    public class ResultadoSocioAmbiental {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoIndicador Tipo { get; set; }
        [NotMapped]
        public string TipoValor {
            get => Enum.GetName(typeof(TipoIndicador), Tipo);
        }
        [Logger]
        public bool? Positivo { get; set; }
        [Logger("Técnica Prevista")]
        public bool? TecnicaPrevista { get; set; }
        [Logger("Descrição")]
        public string Desc { get; set; }
    }
    public enum TipoIndicador {
        ISA1 = 1, ISA2 = 2, ISA3 = 3, ISA4 = 4
    }
}