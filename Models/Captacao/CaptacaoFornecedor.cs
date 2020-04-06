using APIGestor.Models.Fornecedores;

namespace APIGestor.Models.Captacao
{
    public class CaptacaoFornecedor
    {
        public int PropostaConfiguracaoId { get; set; }
        public PropostaConfiguracao PropostaConfiguracao { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
    }
}