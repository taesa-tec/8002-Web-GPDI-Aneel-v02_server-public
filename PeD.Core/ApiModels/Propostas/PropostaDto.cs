using System;
using System.Collections.Generic;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaDto : BaseEntity
    {
        public Guid Guid { get; set; }
        public string Captacao { get; set; }
        public string CaptacaoStatus { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public string ResponsavelId { get; set; }
        public int CaptacaoId { get; set; }
        public short Duracao { get; set; }
        public bool ContratoFinalizado { get; set; }
        public bool PlanoFinalizado { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime? DataTermino { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? DataParticipacao { get; set; }
        public DateTime? DataClausulasAceitas { get; set; }


        public StatusParticipacao Participacao { get; set; }

        public string Consideracoes { get; set; }
        public string PlanoTrabalhoAprovacao { get; set; }
        public string ContratoAprovacao { get; set; }

        public List<CaptacaoArquivoDto> Arquivos { get; set; }
    }

    public abstract class PropostaNodeDto : BaseEntity
    {
        public int PropostaId { get; set; }
    }
}