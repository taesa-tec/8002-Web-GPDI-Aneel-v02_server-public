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
            RuleFor(r => r.Motivacao).NotEmpty();
            RuleFor(r => r.Originalidade).NotEmpty();
            RuleFor(r => r.Aplicabilidade).NotEmpty();
            RuleFor(r => r.Relevancia).NotEmpty();
            RuleFor(r => r.RazoabilidadeCustos).NotEmpty();
            RuleFor(r => r.MetodologiaTrabalho).NotEmpty();
            RuleFor(r => r.BuscaAnterioridade).NotEmpty();
            RuleFor(r => r.Bibliografia).NotEmpty();
            RuleFor(r => r.PesquisasCorrelatasPeDAneel).NotEmpty();
            RuleFor(r => r.PesquisasCorrelatasPeD).NotEmpty();
            RuleFor(r => r.PesquisasCorrelatasExecutora).NotEmpty();
        }
    }
}