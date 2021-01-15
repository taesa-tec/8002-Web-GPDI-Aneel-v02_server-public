using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.FornecedoresDtos
{
    public class ContratoDto : BaseEntity
    {
        public int FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
        public string Conteudo { get; set; }
        public string Status { get; set; }
    }
}