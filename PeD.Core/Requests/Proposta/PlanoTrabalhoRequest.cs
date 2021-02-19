using System.Collections.Generic;

namespace PeD.Core.Requests.Proposta
{
    public class PlanoTrabalhoRequest
    {
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string RazoabilidadeCustos { get; set; }
        public string PesquisasCorrelatas { get; set; }
        public string MetodologiaTrabalho { get; set; }
        public string BuscaAnterioridade { get; set; }
        public string Bibliografia { get; set; }
    }
}