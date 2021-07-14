using FluentValidation;

namespace PeD.Core.Requests.Sistema.Fornecedores
{
    public class FornecedorCreateRequest
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Uf { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelEmail { get; set; }
    }

    public class FornecedorCreateRequestValidator : AbstractValidator<FornecedorCreateRequest>
    {
        public FornecedorCreateRequestValidator()
        {
            RuleFor(f => f.Cnpj).NotEmpty();
            RuleFor(f => f.Nome).NotEmpty();
            RuleFor(f => f.Uf).NotEmpty();
            RuleFor(f => f.ResponsavelNome).NotEmpty();
            RuleFor(f => f.ResponsavelEmail).EmailAddress();
        }
    }
}