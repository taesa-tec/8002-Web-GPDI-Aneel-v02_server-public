using System.ComponentModel.DataAnnotations;
using PeD.Core.Attributes;

namespace PeD.Projetos.Models.Catalogs
{
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