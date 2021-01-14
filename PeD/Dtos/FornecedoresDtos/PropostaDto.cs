using System;
using PeD.Models.Captacao;
using TaesaCore.Models;

namespace PeD.Dtos.FornecedoresDtos
{
    public class PropostaDto : BaseEntity
    {
        public string Captacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataResposta { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int CaptacaoId { get; set; }

        public StatusParticipacao Participacao { get; set; }
        // public List<SugestaoClausula> SugestaoClausulas { get; set; }
    }
}