using TaesaCore.Models;

namespace APIGestor.Dtos.Sistema
{
    public class ClausulaDto : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}