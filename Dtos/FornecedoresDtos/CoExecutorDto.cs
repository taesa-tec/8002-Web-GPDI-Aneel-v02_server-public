using TaesaCore.Models;

namespace APIGestor.Dtos.FornecedoresDtos
{
    public class CoExecutorDto : BaseEntity
    {
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public string CNPJ { get; set; }
        public string UF { get; set; }
        public string RazaoSocial { get; set; }
        public ContratoDto Contrato { get; set; }
    }
}