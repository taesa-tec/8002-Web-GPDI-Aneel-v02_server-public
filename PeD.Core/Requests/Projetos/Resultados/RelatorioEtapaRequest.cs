using System.ComponentModel.DataAnnotations;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class RelatorioEtapaRequest : BaseEntity
    {
        [MaxLength(300)] public string AtividadesRealizadas { get; set; }
    }

    public class RelatorioEtapaRequestValidator : AbstractValidator<RelatorioEtapaRequest>
    {
        public RelatorioEtapaRequestValidator()
        {
            RuleFor(request => request.AtividadesRealizadas).NotEmpty();
        }
    }
}