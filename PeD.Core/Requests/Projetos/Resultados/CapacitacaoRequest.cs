using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using PeD.Core.Utils;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class CapacitacaoRequest : BaseEntity
    {
        public int? ArquivoTrabalhoOrigemId { get; set; }
        public int RecursoId { get; set; }
        public string Tipo { get; set; }
        public bool IsConcluido { get; set; }
        public DateTime? DataConclusao { get; set; }
        [MaxLength(20)] public string CnpjInstituicao { get; set; }
        [MaxLength(50)] public string AreaPesquisa { get; set; }
        [MaxLength(200)] public string TituloTrabalhoOrigem { get; set; }
    }

    public class CapacitacaoRequestValidator : AbstractValidator<CapacitacaoRequest>
    {
        public CapacitacaoRequestValidator()
        {
            RuleFor(r => r.RecursoId).GreaterThan(0);
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.IsConcluido).NotNull();
            RuleFor(r => r.DataConclusao).NotNull();
            RuleFor(r => r.CnpjInstituicao).NotEmpty().Custom((s, context) =>
            {
                if (!CpfCnpj.IsCnpj(s))
                    context.AddFailure("CNPJ inválido");
            });
            RuleFor(r => r.AreaPesquisa).NotEmpty();
            RuleFor(r => r.TituloTrabalhoOrigem).NotEmpty();
        }
    }
}