using TaesaCore.Models;

namespace PeD.Dtos.Sistema
{
    public class ClausulaDto : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}