using PeD.Core.Models.Propostas;

namespace PeD.Core.Requests.Proposta
{
    public class CoExecutorRequest
    {
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string UF { get; set; }
        public CoExecutorFuncao Funcao { get; set; }
    }
}