using FluentValidation;
using PeD.Core.Models.Demandas;

namespace PeD.Core.Requests.Demanda
{
    public class StatusRequest
    {
        public DemandaEtapa Status { get; set; }
    }

    public class StatusRequestValidator : AbstractValidator<StatusRequest>
    {
        public StatusRequestValidator()
        {
            RuleFor(r => r.Status).NotNull();
        }
    }
}