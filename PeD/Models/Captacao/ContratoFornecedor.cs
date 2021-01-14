using TaesaCore.Models;

namespace PeD.Models.Captacao
{
    public class ContratoFornecedor : BaseEntity
    {
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }
        public int PropostaId { get; set; }
        public PropostaFornecedor PropostaFornecedor { get; set; }
        public string Conteudo { get; set; }
        public Contrato.Status Status { get; set; }
    }
}