using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaProdutos")]
    public class Produto : PropostaNode
    {
        public DateTime Created { get; set; }
        public ProdutoClassificacao Classificacao { get; set; }
        public ProdutoTipo Tipo { get; set; }
        public FaseCadeia FaseCadeia { get; set; }
        public string TipoDetalhado { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public enum ProdutoClassificacao
    {
        Intermediario,
        Final
    }

    public enum ProdutoTipo
    {
        CM,
        SW,
        SM,
        MS,
        CD,
        ME
    }

    public enum FaseCadeia
    {
        PB,
        PA,
        DE,
        CS,
        LP,
        IM
    }
}