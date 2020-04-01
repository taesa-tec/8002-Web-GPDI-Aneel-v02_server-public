using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao.Fornecedor
{
    public class CoExecutorDto : BaseEntity
    {
        public int FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public ContratoDto Contrato { get; set; }
    }
}