using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class CoExecutorRequest : BaseEntity
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string UF { get; set; }
        public CoExecutorFuncao Funcao { get; set; }
    }
}