using TaesaCore.Models;

namespace PeD.Core.Models.Captacao
{
    public class Clausula : BaseEntity
    {
        public int Ordem { get; set; }
        public string Conteudo { get; set; }
    }
}