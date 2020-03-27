using System.Collections.Generic;

namespace APIGestor.Models.Captacao.Fornecedor
{
    public class Proposta : BaseEntity
    {
        public enum StatusParticipacao
        {
            Pendente,
            Aceito,
            Rejeitado
        }

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
        public StatusParticipacao Participacao { get; set; }
        public List<SugestaoClausula> SugestaoClausulas { get; set; }
    }
}