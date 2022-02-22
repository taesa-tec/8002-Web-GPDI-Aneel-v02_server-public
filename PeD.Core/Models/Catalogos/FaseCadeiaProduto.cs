using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core.Models.Catalogos
{
    public class FaseCadeiaProduto
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public List<FaseTipoDetalhado> TiposDetalhados { get; set; }
    }

    public class FaseCadeiaProdutoValidator : AbstractValidator<FaseCadeiaProduto>
    {
        public FaseCadeiaProdutoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
        }
    }
}