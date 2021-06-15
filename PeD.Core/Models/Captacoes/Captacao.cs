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
            Cancelada = 0,
            Pendente = 1,
            Elaboracao = 2,
            Fornecedor = 3,
            Encerrada = 4,
            Refinamento = 5, // 2.4
            AnaliseRisco = 6, // 2.5
            Formalizacao = 7 //2.6
        }

        public string Titulo { get; set; }

        public int DemandaId { get; set; }
        public Demanda Demanda { get; set; }

        public int? EspecificacaoTecnicaFileId { get; set; }
        public FileUpload EspecificacaoTecnicaFile { get; set; }

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
            Status == CaptacaoStatus.Cancelada || Status >= CaptacaoStatus.Encerrada &&
            Termino < DateTime.Now;

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
        [ForeignKey("PropostaSelecionadaId")] public Proposta PropostaSelecionada { get; set; }

        #endregion

        #region Identificação de riscos

        /// <summary>
        /// Id do Usuário responsável pela aprovação da proposta
        /// </summary>
        public string UsuarioAprovacaoId { get; set; }

        /// <summary>
        /// Usuário responsável pela aprovação da proposta v2.6
        /// </summary>
        public ApplicationUser UsuarioAprovacao { get; set; }

        /// <summary>
        /// Id do Arquivo comprobatório de identificação de riscos
        /// </summary>
        public int? ArquivoRiscosId { get; set; }

        /// <summary>
        /// Arquivo comprobatório de identificação de riscos
        /// </summary>
        public FileUpload ArquivoRiscos { get; set; }

        #endregion

        #region Formalização

        public bool? IsProjetoAprovado { get; set; }
        public int? ArquivoFormalizacaoId { get; set; }
        public FileUpload ArquivoFormalizacao { get; set; }

        public string UsuarioExecucaoId { get; set; }
        public ApplicationUser UsuarioExecucao { get; set; }

        #endregion
    }
}