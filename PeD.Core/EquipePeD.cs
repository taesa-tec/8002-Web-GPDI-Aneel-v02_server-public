using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core
{
    public class EquipePeD
    {
        public string Diretor { get; set; }
        public string Gerente { get; set; }
        public string Coordenador { get; set; }

        public List<string> Outros { get; set; }

        public List<string> CargosChavesIds
        {
            get { return new List<string> {Diretor, Gerente, Coordenador}; }
        }
    }

    public class EquipePeDValidator : AbstractValidator<EquipePeD>
    {
        public EquipePeDValidator()
        {
            RuleFor(r => r.Diretor).NotEmpty();
            RuleFor(r => r.Gerente).NotEmpty();
            RuleFor(r => r.Coordenador).NotEmpty();
        }
    }
}