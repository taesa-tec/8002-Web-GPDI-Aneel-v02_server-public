using FluentValidation;

namespace PeD.Core.Requests.Demanda
{
    public class SuperiorRequest
    {
        public string SuperiorDireto { get; set; }
    }

    public class SuperiorRequestValidator : AbstractValidator<SuperiorRequest>
    {
        public SuperiorRequestValidator()
        {
            RuleFor(r => r.SuperiorDireto).NotEmpty().NotNull();
        }
    }
}