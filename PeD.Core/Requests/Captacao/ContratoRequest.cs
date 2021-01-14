using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class ContratoRequest : BaseEntity
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Tipo { get; set; }
    }
}