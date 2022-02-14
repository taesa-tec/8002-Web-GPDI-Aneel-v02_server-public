using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class AlocacaoRecursoMaterialRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public int EtapaId { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public int EmpresaRecebedoraId { get; set; }
        public int Quantidade { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
    }

    public class AlocacaoRecursoMaterialRequestValidator : AbstractValidator<AlocacaoRecursoMaterialRequest>
    {
        public AlocacaoRecursoMaterialRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Quantidade).GreaterThan(0);
            RuleFor(r => r.Justificativa).NotEmpty();
        }
    }
}