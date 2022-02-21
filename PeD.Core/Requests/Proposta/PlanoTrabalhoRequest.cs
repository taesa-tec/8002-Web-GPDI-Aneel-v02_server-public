using FluentValidation;

namespace PeD.Core.Requests.Proposta
{
    public class PlanoTrabalhoRequest
    {
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string RazoabilidadeCustos { get; set; }
        public string MetodologiaTrabalho { get; set; }
        public string BuscaAnterioridade { get; set; }
        public string Bibliografia { get; set; }
        public string PesquisasCorrelatasPeDAneel { get; set; }
        public string PesquisasCorrelatasPeD { get; set; }
        public string PesquisasCorrelatasExecutora { get; set; }
    }

    public class PlanoTrabalhoRequestValidator : AbstractValidator<PlanoTrabalhoRequest>
    {
        public PlanoTrabalhoRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Motivacao).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.Originalidade).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.Aplicabilidade).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.Relevancia).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.RazoabilidadeCustos).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.MetodologiaTrabalho).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.BuscaAnterioridade).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.Bibliografia).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.PesquisasCorrelatasPeDAneel).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.PesquisasCorrelatasPeD).NotEmpty().MaximumLength(1000);
            RuleFor(r => r.PesquisasCorrelatasExecutora).NotEmpty().MaximumLength(1000);
        }
    }
}