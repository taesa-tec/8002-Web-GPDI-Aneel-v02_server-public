using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Models.Projetos
{
    public class Projeto : BaseEntity
    {
        #region Novo

        public string Titulo { get; set; }

        public Status Status { get; set; }

        public int PropostaId { get; set; }

        /// <summary>
        /// Proposta de origem do projeto
        /// </summary>
        public Proposta Proposta { get; set; }

        public string Codigo { get; set; }
        public string Numero { get; set; }

        public int? ProponenteId { get; set; }
        public Empresa Proponente { get; set; }

        #endregion

        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataInicioProjeto { get; set; }
        public DateTime DataFinalProjeto { get; set; }

        public int FornecedorId { get; set; }
        public Fornecedores.Fornecedor Fornecedor { get; set; }

        public string ResponsavelId { get; set; }
        [ForeignKey("ResponsavelId")] public ApplicationUser Responsavel { get; set; }

        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }

        public List<ProjetoArquivo> Arquivos { get; set; }
        public List<CoExecutor> CoExecutores { get; set; }
        public List<Produto> Produtos { get; set; }
        public List<Etapa> Etapas { get; set; }

        public List<Risco> Riscos { get; set; }
        public List<RecursoHumano> RecursosHumanos { get; set; }
        public List<RecursoMaterial> RecursosMateriais { get; set; }
        [InverseProperty("Projeto")] public List<RecursoHumano.AlocacaoRh> RecursosHumanosAlocacoes { get; set; }
        [InverseProperty("Projeto")] public List<RecursoMaterial.AlocacaoRm> RecursosMateriaisAlocacoes { get; set; }

        #region Remover?

        public PlanoTrabalho PlanoTrabalho { get; set; }
        public Escopo Escopo { get; set; }
        public ProjetoContrato Contrato { get; set; }
        public int? RelatorioId { get; set; }
        [ForeignKey("RelatorioId")] public Relatorio Relatorio { get; set; }
        public List<Meta> Metas { get; set; }

        #endregion
    }

    public class ProjetoNode : BaseEntity
    {
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
    }
}