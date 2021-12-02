using System;

namespace PeD.Core.Requests.Captacao
{
    public class CaptacaoSelecaoRequest
    {
        public int PropostaId { get; set; }
        public string ResponsavelId { get; set; }
        public DateTime DataAlvo { get; set; }
    }
}