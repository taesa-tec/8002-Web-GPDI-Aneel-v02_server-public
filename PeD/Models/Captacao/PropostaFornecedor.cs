using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Models.Captacao
{
    public enum StatusParticipacao
    {
        Pendente,
        Aceito,
        Rejeitado
    }

    public class PropostaFornecedor : BaseEntity
    {
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
        public bool Finalizado { get; set; }
        public StatusParticipacao Participacao { get; set; }
        public List<SugestaoClausula> SugestaoClausulas { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataResposta { get; set; }
    }
}