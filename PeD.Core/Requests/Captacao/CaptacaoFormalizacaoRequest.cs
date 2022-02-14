using System;
using FluentValidation;
using PeD.Core.Models.Projetos;

namespace PeD.Core.Requests.Captacao
{
    public class CaptacaoFormalizacaoRequest
    {
        public string NumeroProjeto { get; set; }
        public int ArquivoId { get; set; }
        public string ResponsavelId { get; set; }
        public bool Aprovado { get; set; }
        public int? EmpresaProponenteId { get; set; }
        public string SegmentoId { get; set; }
        public string TituloCompleto { get; set; }
        public DateTime? InicioProjeto { get; set; }
        public TipoCompartilhamento? Compartilhamento { get; set; }
    }

    public class CaptacaoFormalizacaoRequestValidator : AbstractValidator<CaptacaoFormalizacaoRequest>
    {
        public CaptacaoFormalizacaoRequestValidator()
        {
            RuleFor(request => request.TituloCompleto).NotEmpty();
        }
    }
}