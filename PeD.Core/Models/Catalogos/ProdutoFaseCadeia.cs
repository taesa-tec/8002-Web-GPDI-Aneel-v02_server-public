using System.Collections.Generic;
using PeD.Core.Attributes;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class ProdutoFaseCadeia : BaseEntity
    {
        [Logger] public string Nome { get; set; }
        [Logger] public string Valor { get; set; }
        [Logger("Tipos detalhados")] public List<CatalogProdutoTipoDetalhado> TiposDetalhados { get; set; }
    }
}