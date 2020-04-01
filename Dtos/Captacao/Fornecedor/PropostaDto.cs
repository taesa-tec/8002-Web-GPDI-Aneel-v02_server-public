using System.Collections.Generic;
using APIGestor.Models.Captacao.Fornecedor;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao.Fornecedor
{
    public class PropostaDto : BaseEntity
    {
        public enum StatusParticipacao
        {
            Pendente,
            Aceito,
            Rejeitado
        }

        public int FornecedorId { get; set; }
        public Models.Fornecedores.Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Models.Captacao.Captacao Captacao { get; set; }
        public StatusParticipacao Participacao { get; set; }
        public List<SugestaoClausula> SugestaoClausulas { get; set; }
    }
}