using System.Collections.Generic;
using FluentValidation;

namespace PeD.Core.Requests.Captacao
{
    public class NovaCaptacaoRequest
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public string UsuarioSuprimentoId { get; set; }
        public string Observacoes { get; set; }
        public List<int> Fornecedores { get; set; }
    }

    public class NovaCaptacaoRequestValidator : AbstractValidator<NovaCaptacaoRequest>
    {
        public NovaCaptacaoRequestValidator()
        {
            RuleFor(r => r.Id).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(r => r.ContratoId).NotNull().GreaterThan(0);
        }
    }
}