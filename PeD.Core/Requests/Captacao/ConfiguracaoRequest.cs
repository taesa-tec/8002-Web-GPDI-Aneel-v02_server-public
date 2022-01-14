using System;
using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class ConfiguracaoRequest : BaseEntity
    {
        public string Consideracoes { get; set; }

        /// <summary>
        /// Data máxima para o envio da proposta
        /// </summary>
        public DateTime Termino { get; set; }

        /// <summary>
        /// Id do contrato
        /// </summary>
        public int ContratoId { get; set; }

        /// <summary>
        /// Ids dos arquivos que devem ser disponibilizados aos fornecedores
        /// </summary>
        public List<int> Arquivos { get; set; }

        /// <summary>
        /// Ids dos fornecedores que serão convidados
        /// </summary>
        public List<int> Fornecedores { get; set; }
    }

    public class ConfiguracaoRequestValidator : AbstractValidator<ConfiguracaoRequest>
    {
        public ConfiguracaoRequestValidator()
        {
            RuleFor(request => request.Fornecedores.Count).GreaterThan(0);
            RuleFor(request => request.ContratoId).GreaterThan(0);
        }
    }
}