using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Attributes;
using PeD.Core.Models.Catalogs;
using CatalogProdutoFaseCadeia = PeD.Projetos.Models.Catalogs.CatalogProdutoFaseCadeia;
using CatalogProdutoTipoDetalhado = PeD.Projetos.Models.Catalogs.CatalogProdutoTipoDetalhado;

namespace PeD.Projetos.Models.Projetos {
    public class Produto {
        public DateTime Created { get; set; }
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        private string _titulo;
        [Logger]
        public string Titulo {
            get => _titulo;
            set => _titulo = value?.Trim();
        }
        [Logger("Descrição")]
        public string Desc { get; set; }
        [Logger("Classificação", "ClassificacaoValor")]
        public ProdutoClassificacao Classificacao { get; set; }
        [NotMapped]
        public string ClassificacaoValor { get => Enum.GetName(typeof(ProdutoClassificacao), Classificacao); }
        [Logger("Tipo", "TipoValor")]
        public ProdutoTipo Tipo { get; set; }
        [NotMapped]
        public string TipoValor { get => Enum.GetName(typeof(ProdutoTipo), Tipo); }
        [Logger("Fase da cadeia", "CatalogProdutoFaseCadeia.Nome")]
        public int? CatalogProdutoFaseCadeiaId { get; set; }
        public CatalogProdutoFaseCadeia CatalogProdutoFaseCadeia { get; set; }
        [Logger("Tipo de produto detalhado", "CatalogProdutoTipoDetalhado.Nome")]
        public int? CatalogProdutoTipoDetalhadoId { get; set; }
        public CatalogProdutoTipoDetalhado CatalogProdutoTipoDetalhado { get; set; }
        [Logger("Fase da cadeia")]
        public ProdutoFaseCadeia? FaseCadeia { get; set; }
        public List<EtapaProduto> EtapaProduto { get; set; }
    }
    public enum ProdutoClassificacao {
        Intermediario,
        Final
    }
    public enum ProdutoTipo {
        CM, SW, SM, MS, CD, ME
    }
    public enum ProdutoFaseCadeia {
        PB, PA, DE, CS, LP, IM
    }
}