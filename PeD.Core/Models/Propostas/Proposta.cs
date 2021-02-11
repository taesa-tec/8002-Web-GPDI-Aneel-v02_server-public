using System;
using System.Collections.Generic;
using PeD.Core.Models.Captacoes;
using TaesaCore.Models;

namespace PeD.Core.Models.Propostas
{
    public enum StatusParticipacao
    {
        Pendente,
        Aceito,
        Rejeitado
    }

    public class Proposta : BaseEntity
    {
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
        public bool Finalizado { get; set; }
        public StatusParticipacao Participacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? DataClausulasAceitas { get; set; }
        
        public List<Contrato> Contratos { get; set; }
    }

    public class PropostaNode : BaseEntity
    {
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
    }
}