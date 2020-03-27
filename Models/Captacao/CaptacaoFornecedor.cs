namespace APIGestor.Models.Captacao
{
    public class CaptacaoFornecedor
    {
        // @todo Chave dessa tabela deve ser os ids combinados -> HasKey(a => new {a.FornecedorId, a.CaptacaoId});
        public int FornecedorId { get; set; }
        public Fornecedor.Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
    }
}