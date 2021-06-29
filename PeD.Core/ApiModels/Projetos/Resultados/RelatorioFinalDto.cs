using PeD.Core.Models;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class RelatorioFinalDto : ProjetoNodeDto
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
        public string TecnicaImplementada { get; set; }

        /// <summary>
        /// APLICABILIDADE PREVISTA FOI ALCANÇADA OU SUPERADA?
        /// </summary>
        public bool IsAplicabilidadeAlcancada { get; set; }

        /// <summary>
        /// JUSTIFICATIVA PELO NÃO ALCANCE DA APLICABILIDADE PREVISTA
        /// </summary>
        public string AplicabilidadeJustificativa { get; set; }

        /// <summary>
        /// DESCRIÇÃO, SUCINTA, DOS RESULTADOS DOS TESTES DE FUNCIONALIDADE DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>

        public string ResultadosTestes { get; set; }

        /// <summary>
        /// DESCRIÇÃO DA ABRANGÊNCIA DE APLICAÇÃO DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>
        public string AbrangenciaProduto { get; set; }

        /// <summary>
        /// DESCRIÇÃO DO ÂMBITO DE APLICAÇÃO DO PRODUTO PRINCIPAL E SUAS RESTRIÇÕES
        /// </summary>
        public string AmbitoAplicacaoProduto { get; set; }

        /// <summary>
        /// DESCRIÇÃO, SUCINTA, DAS ATIVIDADES RELACIONADAS À TRANSFERÊNCIA/DIFUSÃO TECNOLÓGICA
        /// </summary>
        public string TransferenciaTecnologica { get; set; }

        public int RelatorioArquivoId { get; set; }

        public int AuditoriaRelatorioArquivoId { get; set; }
    }
}