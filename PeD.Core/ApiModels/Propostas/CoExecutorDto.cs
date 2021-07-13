using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class CoExecutorDto : PropostaNodeDto
    {
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public string Funcao { get; set; }
        public string Codigo { get; set; }
    }
}