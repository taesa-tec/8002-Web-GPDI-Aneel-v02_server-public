using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class CategoriaContabilAtividadeDto : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int CategoriaContabilId { get; set; }
    }
}