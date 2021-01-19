using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class FaseCadeiaProduto : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<FaseTipoDetalhado> TiposDetalhados { get; set; }
    }
}