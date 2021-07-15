using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class AlocacaoRecursoHumanoRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public int EtapaId { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
    }

    public class AlocacaoRhValidator : AbstractValidator<AlocacaoRecursoHumanoRequest>
    {
        public AlocacaoRhValidator()
        {
            RuleFor(a => a.EmpresaFinanciadoraId).GreaterThan(0);
            RuleFor(a => a.RecursoId).GreaterThan(0);
            RuleFor(a => a.HoraMeses.Sum(hm => hm.Value)).GreaterThan(0).WithMessage("A soma das horas alocadas deve ser maior que zero");
        }
    }
}