using FluentValidation;

namespace PeD.Core.Requests.Demanda
{
    public class RevisorRequest
    {
        public string RevisorId { get; set; }
    }

    public class RevisorRequestValidator : AbstractValidator<RevisorRequest>
    {
        public RevisorRequestValidator()
        {
            RuleFor(r => r.RevisorId).NotEmpty().NotNull();
        }
    }
}