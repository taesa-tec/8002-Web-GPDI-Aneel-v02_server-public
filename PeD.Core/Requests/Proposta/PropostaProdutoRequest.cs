using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class PropostaProdutoRequest : BaseEntity
    {
        public string Classificacao { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string TipoId { get; set; }
        public string FaseCadeiaId { get; set; }
        public int TipoDetalhadoId { get; set; }
    }

    public class PropostaProdutoRequestValidator : AbstractValidator<PropostaProdutoRequest>
    {
        public PropostaProdutoRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Classificacao).NotEmpty();
            RuleFor(r => r.Titulo).NotEmpty();
            RuleFor(r => r.Descricao).NotEmpty();
            RuleFor(r => r.TipoId).NotEmpty();
            RuleFor(r => r.FaseCadeiaId).NotEmpty();
            RuleFor(r => r.TipoDetalhadoId).NotEmpty();
        }
    }
}