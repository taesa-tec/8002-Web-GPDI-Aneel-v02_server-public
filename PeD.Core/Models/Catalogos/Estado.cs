using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class Estado : BaseEntity
    {
        private string _nome;

        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim();
        }

        public string Valor { get; set; }
    }

    public class EstadoValidator : AbstractValidator<Estado>
    {
        public EstadoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.Valor).NotEmpty();
        }
    }
}