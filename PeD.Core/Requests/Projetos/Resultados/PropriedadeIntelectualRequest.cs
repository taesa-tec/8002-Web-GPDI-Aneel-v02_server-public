using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class PropriedadeIntelectualRequest : BaseEntity
    {
        public string Tipo { get; set; }
        public DateTime PedidoData { get; set; }
        [MaxLength(15)] public string PedidoNumero { get; set; }
        [MaxLength(200)] public string TituloINPI { get; set; }
        public List<int> Inventores { get; set; }
        public List<PropriedadeIntelectualDepositanteRequest> Depositantes { get; set; }
    }

    public class PropriedadeIntelectualRequestValidator : AbstractValidator<PropriedadeIntelectualRequest>
    {
        public PropriedadeIntelectualRequestValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.PedidoData).NotNull();
            RuleFor(r => r.PedidoNumero).NotEmpty();
            RuleFor(r => r.Inventores).NotEmpty();
            RuleFor(r => r.Tipo).NotEmpty();
            RuleFor(r => r.TituloINPI).NotEmpty();
            RuleFor(r => r.Depositantes).NotEmpty()
                .Custom((list, context) =>
                {
                    if (list.Sum(d => d.Porcentagem) != 100)
                    {
                        context.AddFailure("Soma do percentual de depositantes é diferente de 100");
                    }
                });
        }
    }
}