using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.Models;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
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