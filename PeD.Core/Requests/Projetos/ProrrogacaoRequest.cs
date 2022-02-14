using System;
using FluentValidation;

namespace PeD.Core.Requests.Projetos
{
    public class ProrrogacaoRequest
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int ProdutoId { get; set; }
    }

    public class ProrrogacaoRequestValidator : AbstractValidator<ProrrogacaoRequest>
    {
        public ProrrogacaoRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.Descricao).NotEmpty();
            RuleFor(r => r.Data).NotNull();
            RuleFor(r => r.ProdutoId).NotNull();
        }
    }
}