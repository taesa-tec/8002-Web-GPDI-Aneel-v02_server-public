using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class RecursoMaterialRequest : BaseEntity
    {
        public string Nome { get; set; }
        public int CategoriaContabilId { get; set; }
        public decimal ValorUnitario { get; set; }
        public string EspecificacaoTecnica { get; set; }
    }

    public class RecursoMaterialRequestValidator : AbstractValidator<RecursoMaterialRequest>
    {
        public RecursoMaterialRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.CategoriaContabilId).NotEmpty();
            RuleFor(r => r.ValorUnitario).NotEmpty();
            RuleFor(r => r.EspecificacaoTecnica).NotEmpty();
        }
    }
}