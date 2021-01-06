using System;
using System.Collections.Generic;
using APIGestor.Models.Captacao;
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

        public DateTime DataCriacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int CaptacaoId { get; set; }

        public StatusParticipacao Participacao { get; set; }
        // public List<SugestaoClausula> SugestaoClausulas { get; set; }
    }
}