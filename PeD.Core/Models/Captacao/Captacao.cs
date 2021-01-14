using System;
using System.Collections.Generic;
using PeD.Core.Models.Demandas;
using TaesaCore.Models;

namespace PeD.Core.Models.Captacao
{
    public class Captacao : BaseEntity
    {
        public enum CaptacaoStatus
        {
            Cancelada,
            Pendente,
            Elaboracao,
            Fornecedor
        }

        public string Titulo { get; set; }

        public int DemandaId { get; set; }
        public Demanda Demanda { get; set; }

        public string CriadorId { get; set; }
        public ApplicationUser Criador { get; set; }
        public string UsuarioSuprimentoId { get; set; }
        public ApplicationUser UsuarioSuprimento { get; set; }

        public int? ContratoSugeridoId { get; set; }
        public Contrato ContratoSugerido { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EnvioCaptacao { get; set; }
        public DateTime? Termino { get; set; }
        public DateTime? Cancelamento { get; set; }

        public CaptacaoStatus Status { get; set; }

        // Para equipe suprimento
        public string Observacoes { get; set; }

        //Para fornecedores
        public string Consideracoes { get; set; }

        public List<CaptacaoSugestaoFornecedor> FornecedoresSugeridos { get; set; }
        public List<CaptacaoFornecedor> FornecedoresConvidados { get; set; }
        public List<CaptacaoContrato> Contratos { get; set; }
        public List<CaptacaoArquivo> Arquivos { get; set; }
        public List<PropostaFornecedor> Propostas { get; set; }
    }
}