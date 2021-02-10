using TaesaCore.Models;

namespace PeD.Core.ApiModels.FornecedoresDtos
{
    public class CoExecutorDto : BaseEntity
    {
        public int PropostaId { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
    }
}