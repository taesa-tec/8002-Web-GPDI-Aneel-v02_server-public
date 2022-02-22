using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace PeD.Core.Models.Catalogos
{
    public class Segmento
    {
        [Key] public string Valor { get; set; }
        public string Nome { get; set; }


        public override string ToString()
        {
            return Nome;
        }
    }

    public class SegmentoValidator : AbstractValidator<Segmento>
    {
        public SegmentoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
        }
    }
}