using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class CatalogProdutoFaseCadeia
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<CatalogProdutoTipoDetalhado> TiposDetalhados { get; set; }
    }
    public class CatalogProdutoTipoDetalhado
    {
        [Key]
        public int Id { get; set; }
        public int CatalogProdutoFaseCadeiaId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }

    }
}