using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class EmpresaRequest : BaseEntity
    {
        public int? EmpresaRefId { get; set; }
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string Codigo { get; set; }
        public string UF { get; set; }
        public Funcao Funcao { get; set; }
    }
}