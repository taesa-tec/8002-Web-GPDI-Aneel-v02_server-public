using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class IndicadorEconomicoRequest : BaseEntity
    {
        public string Tipo { get; set; }
        [MaxLength(400)] public string Beneficio { get; set; }
        [MaxLength(10)] public string UnidadeBase { get; set; }
        public decimal ValorNumerico { get; set; }
        public decimal PorcentagemImpacto { get; set; }
        public decimal ValorBeneficio { get; set; }
    }

    public class IndicadorEconomicoRequestValidor : AbstractValidator<IndicadorEconomicoRequest>
    {
        public IndicadorEconomicoRequestValidor()
        {
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.Beneficio).NotEmpty();
            RuleFor(r => r.UnidadeBase).NotEmpty();
            RuleFor(r => r.ValorNumerico).GreaterThan(0);
            RuleFor(r => r.PorcentagemImpacto).GreaterThan(0);
            RuleFor(r => r.ValorBeneficio).GreaterThan(0);
        }
    }
}