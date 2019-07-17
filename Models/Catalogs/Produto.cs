using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APIGestor.Attributes;

namespace APIGestor.Models {
    public class CatalogProdutoFaseCadeia {
        [Key]
        public int Id { get; set; }
        [Logger]
        public string Nome { get; set; }
        [Logger]
        public string Valor { get; set; }
        [Logger("Tipos detalhados")]
        public List<CatalogProdutoTipoDetalhado> TiposDetalhados { get; set; }
    }
    public class CatalogProdutoTipoDetalhado {
        [Key]
        public int Id { get; set; }
        public int CatalogProdutoFaseCadeiaId { get; set; }
        [Logger]
        public string Nome { get; set; }
        [Logger]
        public string Valor { get; set; }

    }
}