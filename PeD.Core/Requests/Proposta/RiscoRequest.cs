using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class RiscoRequest : BaseEntity
    {
        public string Item { get; set; }
        public string Classificacao { get; set; }
        public string Justificativa { get; set; }
        public string Probabilidade { get; set; }
    }

    public class RiscoRequestValidator : AbstractValidator<RiscoRequest>
    {
        public RiscoRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Item).NotEmpty();
            RuleFor(r => r.Classificacao).NotEmpty();
            RuleFor(r => r.Justificativa).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.Probabilidade).NotEmpty();
        }
    }
}