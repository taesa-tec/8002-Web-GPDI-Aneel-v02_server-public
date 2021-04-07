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
        public string Observacoes { get; set; }

        //Para fornecedores
        public string Consideracoes { get; set; }

        public int? TemaId { get; set; }
        public Tema Tema { get; set; }
        public string TemaOutro { get; set; }

        public PropostaSelecao Selecao { get; set; }

        public List<CaptacaoSubTema> SubTemas { get; set; }
        public List<CaptacaoSugestaoFornecedor> FornecedoresSugeridos { get; set; }
        public List<CaptacaoFornecedor> FornecedoresConvidados { get; set; }
        public List<CaptacaoArquivo> Arquivos { get; set; }
        public List<Proposta> Propostas { get; set; }
    }
}