using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PeD.Core.Attributes;

namespace PeD.Projetos.Models.Projetos {
    public class RelatorioFinal {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        [Logger("Produto Alcançado")]
        public bool? ProdutoAlcancado { get; set; }
        [Logger("Justificativa do Produto")]
        public string JustificativaProduto { get; set; }
        [Logger("Especificação do produto")]
        public string EspecificacaoProduto { get; set; }

        [Logger("Técnica prevista implementada")]
        public bool? TecnicaPrevista { get; set; }
        [Logger("Justificativa da técnica")]
        public string JustificativaTecnica { get; set; }
        [Logger("Descrição técnica")]
        public string DescTecnica { get; set; }
        [Logger("Aplicabilidade prevista alcançada")]
        public bool? AplicabilidadePrevista { get; set; }
        [Logger("Justificativa da aplicabilidade")]
        public string JustificativaAplicabilidade { get; set; }
        [Logger("Descrição dos resultados dos testes")]
        public string DescTestes { get; set; }
        [Logger("Descrição da abrangência")]
        public string DescAbrangencia { get; set; }
        [Logger("Descrição do âmbito de aplicação")]
        public string DescAmbito { get; set; }
        [Logger("Descrição de atividades")]
        public string DescAtividades { get; set; }

        public int ProjetoId { get; set; }
        public ICollection<Upload> Uploads { get; set; }
    }
}