using APIGestor.Models.Captacao;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao.Fornecedor
{
    public class ContratoDto : BaseEntity
    {
        public int FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public int PropostaId { get; set; }
        public PropostaFornecedor PropostaFornecedor { get; set; }
        public string Conteudo { get; set; }
        public string Status { get; set; }
    }
}