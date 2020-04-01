using TaesaCore.Models;

namespace APIGestor.Models.Captacao
{
    public class ContratoFornecedor : BaseEntity
    {
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
        public string Conteudo { get; set; }
        public Contrato.Status Status { get; set; }
    }
}