using PeD.Models.Captacao;
using TaesaCore.Models;

namespace PeD.Dtos.FornecedoresDtos
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