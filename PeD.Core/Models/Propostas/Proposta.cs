using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Captacoes;
using TaesaCore.Models;

namespace PeD.Core.Models.Propostas
{
    public enum StatusParticipacao
    {
        Pendente = 0,
        Aceito = 1,
        Rejeitado = 2,
        Concluido = 3,
        Cancelado = 4,
    }


    public enum StatusAprovacao
    {
        Pendente = 0,
        Alteracao = 1,
        Aprovado = 2
    }

    public class Proposta : BaseEntity
    {
        public Guid Guid { get; set; }
        public bool Finalizado { get; set; }
        public StatusParticipacao Participacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime? DataParticipacao { get; set; }
        public DateTime? DataClausulasAceitas { get; set; }


        public int? RelatorioId { get; set; }
        [ForeignKey("RelatorioId")] public Relatorio Relatorio { get; set; }
        public short Duracao { get; set; }
        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }

        public string ResponsavelId { get; set; }
        [ForeignKey("ResponsavelId")] public ApplicationUser Responsavel { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }

        public PlanoTrabalho PlanoTrabalho { get; set; }
        public StatusAprovacao PlanoTrabalhoAprovacao { get; set; }
        public Escopo Escopo { get; set; }
        public PropostaContrato Contrato { get; set; }
        public StatusAprovacao ContratoAprovacao { get; set; }
        public List<PropostaArquivo> Arquivos { get; set; }
        public List<Empresa> Empresas { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<Etapa> Etapas { get; set; }
        public List<Meta> Metas { get; set; }
        public List<Risco> Riscos { get; set; }
        public List<RecursoHumano> RecursosHumanos { get; set; }
        public List<RecursoMaterial> RecursosMateriais { get; set; }
        [InverseProperty("Proposta")] public List<RecursoHumano.AlocacaoRh> RecursosHumanosAlocacoes { get; set; }
        [InverseProperty("Proposta")] public List<RecursoMaterial.AlocacaoRm> RecursosMateriaisAlocacoes { get; set; }
        public List<Relatorio> HistoricoRelatorios { get; set; }
        public List<ContratoComentario> ContratoComentarios { get; set; }
        public List<PlanoComentario> PlanoTrabalhoComentarios { get; set; }
    }

    public class PropostaNode : BaseEntity
    {
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
    }
}