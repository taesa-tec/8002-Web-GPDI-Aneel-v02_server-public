using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class FaseCadeiaProdutoDto : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<FaseTipoDetalhadoDto> TiposDetalhados { get; set; }
    }
}