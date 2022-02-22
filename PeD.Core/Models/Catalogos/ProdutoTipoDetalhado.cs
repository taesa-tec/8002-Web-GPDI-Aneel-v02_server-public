using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class FaseTipoDetalhado : BaseEntity
    {
        [Required] public string FaseCadeiaProdutoId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }

    public class FaseTipoDetalhadoValidator : AbstractValidator<FaseTipoDetalhado>
    {
        public FaseTipoDetalhadoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.Valor).NotEmpty();
        }
    }
}