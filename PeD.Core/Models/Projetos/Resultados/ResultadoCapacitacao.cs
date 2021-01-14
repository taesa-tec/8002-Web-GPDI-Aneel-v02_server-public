using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;

namespace PeD.Core.Models.Projetos.Resultados {
    public class ResultadoCapacitacao {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Recurso Humanos", "RecursoHumano.NomeCompleto")]
        public int RecursoHumanoId { get; set; }
        public RecursoHumano RecursoHumano { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoCapacitacao Tipo { get; set; }
        [NotMapped]
        public string TipoValor {
            get => Enum.GetName(typeof(TipoCapacitacao), Tipo);
        }
        [Logger("Conclusão")]
        public bool? Conclusao { get; set; }
        [Logger("Data da conclusão")]
        public DateTime DataConclusao { get; set; }
        [Logger("Cnpj da Instituição")]
        public string CnpjInstituicao { get; set; }
        [Logger("Área de pesquisa")]
        public string AreaPesquisa { get; set; }
        [Logger("Titulo do trabalho")]
        public string TituloTrabalho { get; set; }

        public ICollection<Upload> Uploads { get; set; }
    }
    public enum TipoCapacitacao {
        PD = 1, DO = 2, ME = 3, ES = 4
    }
}