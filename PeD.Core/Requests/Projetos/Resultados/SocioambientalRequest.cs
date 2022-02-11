using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class SocioambientalRequest : BaseEntity
    {
        public string Tipo { get; set; }
        public bool ResultadoPositivo { get; set; }
        [MaxLength(500)] public string DescricaoResultado { get; set; }
    }

    public class SocioambientalRequestValidator : AbstractValidator<SocioambientalRequest>
    {
        public SocioambientalRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.DescricaoResultado).NotEmpty();
        }
    }
}