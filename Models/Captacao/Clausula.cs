using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class Clausula : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}