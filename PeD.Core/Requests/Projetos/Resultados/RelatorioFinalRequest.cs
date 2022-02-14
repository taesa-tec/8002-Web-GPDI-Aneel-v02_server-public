using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class RelatorioFinalRequest : BaseEntity
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
        public int? AuditoriaRelatorioArquivoId { get; set; }
    }

    public class RelatorioFinalRequestValidator : AbstractValidator<RelatorioFinalRequest>
    {
        public RelatorioFinalRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.AbrangenciaProduto).NotEmpty().When(r => r.IsAplicabilidadeAlcancada);
            RuleFor(r => r.AmbitoAplicacaoProduto).NotEmpty().When(r => r.IsAplicabilidadeAlcancada);
            RuleFor(r => r.ResultadosTestes).NotEmpty().When(r => r.IsAplicabilidadeAlcancada);
            RuleFor(r => r.AplicabilidadeJustificativa).NotEmpty().When(r => !r.IsAplicabilidadeAlcancada);
            RuleFor(r => r.TecnicaImplementada).NotEmpty();
            RuleFor(r => r.TecnicaProduto).NotEmpty();
            RuleFor(r => r.TransferenciaTecnologica).NotEmpty();
        }
    }
}