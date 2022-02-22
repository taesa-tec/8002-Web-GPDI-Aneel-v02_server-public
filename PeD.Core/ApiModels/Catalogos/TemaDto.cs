using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class TemaDto : BaseEntity
    {
        public int? ParentId { get; set; }
        public string Parent { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<TemaDto> SubTemas { get; set; }
        public int Order { get; set; }
    }

    public class TemaDtoValidator : AbstractValidator<TemaDto>
    {
        public TemaDtoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
            RuleFor(r => r.Valor).NotEmpty();
        }
    }
}