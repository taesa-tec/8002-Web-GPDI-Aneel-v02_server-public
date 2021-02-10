using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class FaseTipoDetalhadoDto : BaseEntity
    {
        public int FaseCadeiaProdutoId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}