using APIGestor.Models.Fornecedores;

namespace APIGestor.Models.Captacao
{
    public class CaptacaoFornecedor
    {
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}