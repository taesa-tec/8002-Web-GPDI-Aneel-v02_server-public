using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;
using PeD.Core.Models.Catalogs;
using Pais = PeD.Projetos.Models.Catalogs.Pais;

namespace PeD.Projetos.Models.Projetos.Resultados {
    public class ResultadoProducao {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Tipo", "TipoValor")]
        public TipoProducao Tipo { get; set; }
        [NotMapped]
        public string TipoValor {
            get => Enum.GetName(typeof(TipoProducao), Tipo);
        }
        [Logger("Data de publicação")]
        public DateTime DataPublicacao { get; set; }
        [Logger("Confirmação")]
        public bool? Confirmacao { get; set; }
        [Logger]
        public string Nome { get; set; }
        [Logger]
        public string Url { get; set; }
        [Logger("País", "Pais.Nome")]
        public int CatalogPaisId { get; set; }
        [ForeignKey("CatalogPaisId")]
        public Pais Pais { get; set; }
        [Logger]
        public string Cidade { get; set; }
        [Logger]
        public string Titulo { get; set; }
        public ICollection<Upload> Uploads { get; set; }
    }

    public enum TipoProducao {
        PN = 1, PI = 2, AN = 3, AI = 4
    }
}