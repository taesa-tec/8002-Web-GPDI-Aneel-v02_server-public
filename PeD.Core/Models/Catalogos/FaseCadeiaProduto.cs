using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class FaseCadeiaProduto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public List<FaseTipoDetalhado> TiposDetalhados { get; set; }
    }
}