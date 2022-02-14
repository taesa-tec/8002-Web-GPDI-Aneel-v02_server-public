using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class ProducaoCientificaRequest : BaseEntity
    {
        public string Tipo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool ConfirmacaoPublicacao { get; set; }
        [MaxLength(50)] public string NomeEventoPublicacao { get; set; }

        [MaxLength(50)] public string LinkPublicacao { get; set; }

        public int? ArquivoTrabalhoOrigemId { get; set; }
        public int PaisId { get; set; }
        [MaxLength(30)] public string Cidade { get; set; }
        [MaxLength(200)] public string TituloTrabalho { get; set; }
    }

    public class ProducaoCientificaRequestValidator : AbstractValidator<ProducaoCientificaRequest>
    {
        public ProducaoCientificaRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.TituloTrabalho).NotEmpty();
            RuleFor(r => r.DataPublicacao).NotNull();
            RuleFor(r => r.ConfirmacaoPublicacao).NotNull();
            RuleFor(r => r.Cidade).NotEmpty();
            RuleFor(r => r.NomeEventoPublicacao).NotEmpty();
            RuleFor(r => r.PaisId).GreaterThan(0);
        }
    }
}