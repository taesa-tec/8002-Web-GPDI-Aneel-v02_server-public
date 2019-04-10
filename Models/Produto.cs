using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Produto
    {
        public DateTime Created { get; set; }
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => _titulo = value?.Trim();
        }
        public string Desc { get; set; }
        public ProdutoClassificacao Classificacao { get; set; }   
        [NotMapped]
        public string ClassificacaoValor { get => Enum.GetName(typeof(ProdutoClassificacao),Classificacao); }
        public ProdutoTipo Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Enum.GetName(typeof(ProdutoTipo),Tipo); }
        public int? CatalogProdutoFaseCadeiaId { get; set; }
        public CatalogProdutoFaseCadeia CatalogProdutoFaseCadeia { get; set; }
        public int? CatalogProdutoTipoDetalhadoId { get; set; }
        public CatalogProdutoTipoDetalhado CatalogProdutoTipoDetalhado { get; set; }
        public ProdutoFaseCadeia? FaseCadeia { get; set; }
        [NotMapped]
        public List<EtapaProduto> EtapaProduto { get; set; }
    }
    public enum ProdutoClassificacao
    {
        Intermediario,
        Final
    }
    public enum ProdutoTipo
    {
        CM, SW, SM, MS, CD, ME
    }
    public enum ProdutoFaseCadeia
    {
        PB, PA, DE, CS, LP, IM
    }
}