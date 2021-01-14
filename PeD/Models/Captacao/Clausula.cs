using TaesaCore.Models;

namespace PeD.Models.Captacao
{
    public class Clausula : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}