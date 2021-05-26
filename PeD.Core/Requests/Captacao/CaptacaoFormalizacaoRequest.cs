using System;

namespace PeD.Core.Requests.Captacao
{
    public class CaptacaoFormalizacaoRequest
    {
        public string NumeroProjeto { get; set; }
        public string ResponsavelId { get; set; }
        public bool Aprovado { get; set; }
        public int EmpresaProponenteId { get; set; }
        public string TituloCompleto { get; set; }
        public DateTime InicioProjeto { get; set; }
    }
}