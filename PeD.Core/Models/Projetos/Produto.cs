using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Catalogos;

namespace PeD.Core.Models.Projetos
{
    public enum ProdutoClassificacao
    {
        Intermediario,
        Final
    }

    [Table("ProjetoProdutos")]
    public class Produto : ProjetoNode
    {
        public DateTime Created { get; set; }
        public ProdutoClassificacao Classificacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        public string TipoId { get; set; }
        [ForeignKey("TipoId")] public ProdutoTipo ProdutoTipo { get; set; }


        public string FaseCadeiaId { get; set; }
        [ForeignKey("FaseCadeiaId")] public FaseCadeiaProduto FaseCadeia { get; set; }

        [Required] public int TipoDetalhadoId { get; set; }
        [ForeignKey("TipoDetalhadoId")] public FaseTipoDetalhado TipoDetalhado { get; set; }
    }
}