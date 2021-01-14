using TaesaCore.Models;

namespace PeD.Dtos.Captacao
{
    public class ContratoDto : BaseEntity
    {
        public string Titulo { get; set; }

        public string Header { get; set; }
        public string Conteudo { get; set; }
        public string Footer { get; set; }
    }
}