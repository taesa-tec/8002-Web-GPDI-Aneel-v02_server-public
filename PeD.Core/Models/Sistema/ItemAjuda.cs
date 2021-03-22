using TaesaCore.Models;

namespace PeD.Core.Models.Sistema
{
    public class ItemAjuda : BaseEntity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Conteudo { get; set; }
        public string Descricao { get; set; }
    }
}