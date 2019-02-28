using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class RelatorioFinal
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public bool? ProdutoAlcancado { get; set; }
        public string JustificativaProduto { get;set; }
        public string EspecificacaoProduto { get;set; }
        public bool? TecnicaPrevista { get; set; }
        public string JustificativaTecnica { get;set; }
        public string DescTecnica { get;set; }
        public bool? AplicabilidadePrevista { get; set; }
        public string JustificativaAplicabilidade { get;set; }
        public string DescTestes { get;set; }
        public string DescAbrangencia { get;set; }
        public string DescAmbito { get;set; }
        public string DescAtividades { get;set; }
        public int ProjetoId { get;set; }
        public ICollection<Upload> Uploads { get; set; }
    }
}