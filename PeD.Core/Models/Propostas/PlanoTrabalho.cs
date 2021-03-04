using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaPlanosTrabalhos")]
    public class PlanoTrabalho : PropostaNode
    {
        public string Motivacao { get; set; } = "";
        public string Originalidade { get; set; } = "";
        public string Aplicabilidade { get; set; } = "";
        public string Relevancia { get; set; } = "";
        public string RazoabilidadeCustos { get; set; } = "";
        public string MetodologiaTrabalho { get; set; } = "";
        public string BuscaAnterioridade { get; set; } = "";
        public string Bibliografia { get; set; } = "";
        
        public string PesquisasCorrelatasPeDAneel { get; set; } = "";
        public string PesquisasCorrelatasPeD { get; set; } = "";
        public string PesquisasCorrelatasExecutora { get; set; } = "";
    }
}