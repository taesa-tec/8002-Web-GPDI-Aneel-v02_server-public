using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PeD.Core.Attributes;

namespace PeD.Projetos.Models.Catalogs {
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
}