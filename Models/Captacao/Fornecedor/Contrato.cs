namespace APIGestor.Models.Captacao.Fornecedor
{
    public class Contrato : BaseEntity
    {
        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
        public string Conteudo { get; set; }
    }
}