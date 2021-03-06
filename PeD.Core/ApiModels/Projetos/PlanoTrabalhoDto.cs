using System.Collections.Generic;
using PeD.Core.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class PlanoTrabalhoDto
    {
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string RazoabilidadeCustos { get; set; }
        public string PesquisasCorrelatasPeDAneel { get; set; } = "";
        public string PesquisasCorrelatasPeD { get; set; } = "";
        public string PesquisasCorrelatasExecutora { get; set; } = "";
        public string MetodologiaTrabalho { get; set; }
        public string BuscaAnterioridade { get; set; }
        public string Bibliografia { get; set; }
        public List<FileUpload> Arquivos { get; set; }
    }
}