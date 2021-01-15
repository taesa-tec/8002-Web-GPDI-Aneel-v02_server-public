namespace PeD.Core.Models.Captacoes
{
    public class CaptacaoSugestaoFornecedor
    {
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
    }
}