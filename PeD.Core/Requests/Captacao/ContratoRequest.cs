using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class ContratoRequest : BaseEntity
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public string Tipo { get; set; }
    }

    public class ContratoRequestValidator : AbstractValidator<ContratoRequest>
    {
        public ContratoRequestValidator()
        {
            RuleFor(request => request.Titulo).NotEmpty();
            RuleFor(request => request.Conteudo).NotEmpty();
            RuleFor(request => request.Tipo).NotEmpty();
        }
    }
}