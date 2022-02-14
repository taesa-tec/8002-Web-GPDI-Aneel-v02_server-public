using FluentValidation;
using PeD.Core.Utils;

namespace PeD.Core.Requests.Users
{
    public class EditMeRequest
    {
        public string NomeCompleto { get; set; }
        public string Cargo { get; set; }
        public string Cpf { get; set; }
    }

    public class EditMeRequestValidator : AbstractValidator<EditMeRequest>
    {
        public EditMeRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.NomeCompleto).NotEmpty();
            RuleFor(r => r.Cpf).Custom((s, context) =>
            {
                if (!CpfCnpj.IsCpf(s))
                {
                    context.AddFailure("Cpf Inválido");
                }
            });
        }
    }
}