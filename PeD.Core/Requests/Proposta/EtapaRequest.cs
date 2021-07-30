using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class EtapaRequest : BaseEntity
    {
        public string DescricaoAtividades { get; set; }
        public int ProdutoId { get; set; }
        public int MesInicio { get; set; }
        public int MesFinal { get; set; }
    }

    public class EtapaRequestValidator : AbstractValidator<EtapaRequest>
    {
        public EtapaRequestValidator()
        {
            RuleFor(r => r).Custom((r, c) =>
            {
                if (r.MesFinal - r.MesInicio < 2)
                {
                    c.AddFailure("Duração mínima da etapa é de 3 meses");
                }

                // if (r.MesFinal - r.MesInicio > 5)
                // {
                //     c.AddFailure("Duração máxima da etapa é de 6 meses");
                // }
            });
        }
    }
}