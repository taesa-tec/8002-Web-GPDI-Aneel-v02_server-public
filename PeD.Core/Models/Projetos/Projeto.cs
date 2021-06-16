using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Catalogos;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.Models.Projetos
{
    public class Projeto : BaseEntity
    {
        public string Titulo { get; set; }
        public string TituloCompleto { get; set; }

        public int PlanoTrabalhoFileId { get; set; }
        public FileUpload PlanoTrabalhoFile { get; set; }

        public int ContratoId { get; set; }
        public FileUpload Contrato { get; set; }

        public int? EspecificacaoTecnicaFileId { get; set; }
        public FileUpload EspecificacaoTecnicaFile { get; set; }
        public Status Status { get; set; }

        public int? TemaId { get; set; }
        public Tema Tema { get; set; }
        public string TemaOutro { get; set; }

        public int PropostaId { get; set; }

        /// <summary>
        /// Proposta de origem do projeto
        /// </summary>
        public Proposta Proposta { get; set; }

        public string Codigo { get; set; }
        public string Numero { get; set; }

        public int? ProponenteId { get; set; }
        public Empresa Proponente { get; set; }

        /// <summary>
        /// Data de entrada do proposta no sitema
        /// </summary>
        public DateTime DataCriacao { get; set; }

        /// <summary>
        /// Data de alteraçõa dos dados do projeto
        /// </summary>
        public DateTime DataAlteracao { get; set; }

        /// <summary>
        /// Data Real do início do projeto
        /// </summary>
        public DateTime DataInicioProjeto { get; set; }

        /// <summary>
        /// Data prevista para o encerramento do projeto
        /// </summary>
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
        [InverseProperty("Projeto")] public List<Alocacao> Alocacoes { get; set; }

        #region Remover?

        public PlanoTrabalho PlanoTrabalho { get; set; }
        public Escopo Escopo { get; set; }
        public List<Meta> Metas { get; set; }

        #endregion
    }


    public class ProjetoNode : BaseEntity
    {
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
    }

    public class ProjetoXml : ProjetoNode
    {
        public enum TipoXml
        {
            None,
            Prorrogacao,
            Duto,
            RelatorioFinal,
            Auditoria
        }
        public int FileId { get; set; }
        public FileUpload File { get; set; }
        public string Versao { get; set; }
        public TipoXml Tipo { get; set; }
    }
}