namespace APIGestor.Models.Captacao.Fornecedor
{
    public class SugestaoClausula : BaseEntity
    {
        public int ClausulaId { get; set; }
        public Clausula Clausula { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedor Type { get; set; }
        public string Conteudo { get; set; }
    }
}