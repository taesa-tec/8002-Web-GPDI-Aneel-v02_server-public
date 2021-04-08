using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Demandas;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Models.Captacoes
{
    public class Captacao : BaseEntity
    {
        public enum CaptacaoStatus
        {
            Cancelada,
            Pendente,
            Elaboracao,
            Fornecedor,
            Encerrada
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

        public int? ContratoId { get; set; }
        [ForeignKey("ContratoId")] public Contrato Contrato { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? EnvioCaptacao { get; set; }
        public DateTime? Termino { get; set; }
        public DateTime? Cancelamento { get; set; }

        public CaptacaoStatus Status { get; set; }

        [NotMapped]
        public bool IsPropostasOpen =>
            Status == CaptacaoStatus.Cancelada || Status == CaptacaoStatus.Encerrada &&
            Termino < DateTime.Now;

        // Para equipe suprimento
        /// <summary>
        /// Observações para equipe de suprimentos
        /// </summary>
        public string Observacoes { get; set; }

        /// <summary>
        /// Considerações para os fornecedores
        /// </summary>
        public string Consideracoes { get; set; }

        #region Temas

        public int? TemaId { get; set; }
        public Tema Tema { get; set; }
        public string TemaOutro { get; set; }

        public List<CaptacaoSubTema> SubTemas { get; set; }

        #endregion

        public List<CaptacaoSugestaoFornecedor> FornecedoresSugeridos { get; set; }
        public List<CaptacaoFornecedor> FornecedoresConvidados { get; set; }
        public List<CaptacaoArquivo> Arquivos { get; set; }

        /// <summary>
        /// Propostas Recebidas
        /// </summary>
        public List<Proposta> Propostas { get; set; }

        #region Seleção de proposta

        public string UsuarioRefinamentoId { get; set; }
        public ApplicationUser UsuarioRefinamento { get; set; }

        public DateTime? DataAlvo { get; set; }

        public int? ArquivoComprobatorioId { get; set; }
        public FileUpload ArquivoComprobatorio { get; set; }

        public int? PropostaSelecionadaId { get; set; }
        [ForeignKey("PropostaSelecionadaId")]
        public Proposta PropostaSelecionada { get; set; }

        #endregion
    }
}