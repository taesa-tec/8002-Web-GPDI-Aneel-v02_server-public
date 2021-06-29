using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Projetos.Resultados
{
    public class RelatorioFinal : ProjetoNode
    {
        /// <summary>
        /// PRODUTO PRINCIPAL PREVISTO FOI ALCANÇADO OU SUPERADO?
        /// </summary>
        public bool IsProdutoAlcancado { get; set; }

        /// <summary>
        /// JUSTIFICATIVA PELO NÃO ALCANCE DO PRODUTO PRINCIPAL PREVISTO
        /// OU
        /// ESPECIFICAÇÃO TÉCNICA DO PRODUTO PRINCIPAL OBTIDO
        /// </summary>
        [MaxLength(1000)]
        public string TecnicaProduto { get; set; }

        /// <summary>
        /// TÉCNICA PREVISTA FOI IMPLEMENTADA?
        /// </summary>
        public bool IsTecnicaImplementada { get; set; }

        /// <summary>
        /// JUSTIFICATIVA PELA NÃO IMPLEMENTAÇÃO DA TÉCNICA PREVISTA
        /// OU
        /// DESCRIÇÃO, SUCINTA, DA TÉCNICA EMPREGADA
        /// </summary>
        [MaxLength(1000)]
        public string TecnicaImplementada { get; set; }

        /// <summary>
        /// APLICABILIDADE PREVISTA FOI ALCANÇADA OU SUPERADA?
        /// </summary>
        public bool IsAplicabilidadeAlcancada { get; set; }

        /// <summary>
        /// JUSTIFICATIVA PELO NÃO ALCANCE DA APLICABILIDADE PREVISTA
        /// </summary>
        [MaxLength(1000)]
        public string AplicabilidadeJustificativa { get; set; }

        /// <summary>
        /// DESCRIÇÃO, SUCINTA, DOS RESULTADOS DOS TESTES DE FUNCIONALIDADE DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>

        [MaxLength(1000)]
        public string ResultadosTestes { get; set; }

        /// <summary>
        /// DESCRIÇÃO DA ABRANGÊNCIA DE APLICAÇÃO DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>
        [MaxLength(1000)]
        public string AbrangenciaProduto { get; set; }

        /// <summary>
        /// DESCRIÇÃO DO ÂMBITO DE APLICAÇÃO DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>
        [MaxLength(1000)]
        public string AmbitoAplicacaoProduto { get; set; }

        /// <summary>
        /// DESCRIÇÃO, SUCINTA, DAS ATIVIDADES RELACIONADAS À TRANSFERÊNCIA/DIFUSÃO TECNOLÓGICA
        /// </summary>
        [MaxLength(500)]
        public string TransferenciaTecnologica { get; set; }

        public int? RelatorioArquivoId { get; set; }

        /// <summary>
        /// ARQUIVO PDF RELATÓRIO FINAL
        /// </summary>
        public FileUpload RelatorioArquivo { get; set; }

        public int? AuditoriaRelatorioArquivoId { get; set; }

        /// <summary>
        /// ARQUIVO EM FORMATO PDF DO RELATÓRIO DE AUDITORIA CONTÁBIL E FINANCEIRA DO PROJETO DE P&D
        /// </summary>
        public FileUpload AuditoriaRelatorioArquivo { get; set; }
    }
}