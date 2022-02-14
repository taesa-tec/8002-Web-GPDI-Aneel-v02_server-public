using FluentValidation;

namespace PeD.Core.Requests.Projetos
{
    public class XmlRequest
    {
        public string Versao { get; set; }
    }

    public class XmlRequestValidator : AbstractValidator<XmlRequest>
    {
        public XmlRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Versao).NotEmpty();
            
        }
    }
}