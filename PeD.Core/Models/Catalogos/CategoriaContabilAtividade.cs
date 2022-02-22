using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class CategoriaContabilAtividade : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int CategoriaContabilId { get; set; }
    }

    public class CategoriaContabilAtividadeValidator : AbstractValidator<CategoriaContabilAtividade>
    {
        public CategoriaContabilAtividadeValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.Valor).NotEmpty();
        }
    }
}