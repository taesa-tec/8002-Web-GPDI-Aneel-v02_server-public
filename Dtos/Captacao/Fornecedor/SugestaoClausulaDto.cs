using APIGestor.Models.Captacao;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao.Fornecedor
{
    public class SugestaoClausulaDto : BaseEntity
    {
        public int ClausulaId { get; set; }
        public Clausula Clausula { get; set; }
        public int FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public string Conteudo { get; set; }
    }
}