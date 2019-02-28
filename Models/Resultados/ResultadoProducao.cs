using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ResultadoProducao
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get;set; }
        public TipoProducao Tipo { get; set; }
        [NotMapped]
        public string TipoValor { 
            get => Enum.GetName(typeof(TipoProducao),Tipo);
        }
        public DateTime DataPublicacao { get; set; }
        public bool? Confirmacao { get;set; }
        public string Nome { get;set; }
        public string Url { get; set; }
        public int CatalogPaisId { get; set; }
        [ForeignKey("CatalogPaisId")]
        public CatalogPais Pais { get;set; }
        public string Cidade { get;set; }
        public string Titulo { get; set; }
        public ICollection<Upload> Uploads { get; set; }
    }

    public enum TipoProducao
    {
        PN=1,PI=2,AN=3,AI=4
    }
}