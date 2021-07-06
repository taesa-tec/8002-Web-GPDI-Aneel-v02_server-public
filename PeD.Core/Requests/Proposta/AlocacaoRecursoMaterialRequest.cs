using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.Requests.Proposta
{
    public class AlocacaoRecursoMaterialRequest : BaseEntity
    {
        public int RecursoId { get; set; }
        public int EtapaId { get; set; }
        public int? EmpresaFinanciadoraId { get; set; }
        public int? CoExecutorFinanciadorId { get; set; }
        public int? EmpresaRecebedoraId { get; set; }
        public int? CoExecutorRecebedorId { get; set; }
        public int Quantidade { get; set; }
        public string Justificativa { get; set; }
        public Dictionary<short, short> HoraMeses { get; set; }
    }

    public class AlocacaoRecursoMaterialRequestValidator : AbstractValidator<AlocacaoRecursoMaterialRequest>
    {
        public AlocacaoRecursoMaterialRequestValidator()
        {
            RuleFor(a => a).Custom((alocacao, context) =>
            {
                if (alocacao.CoExecutorFinanciadorId != null && alocacao.EmpresaRecebedoraId != null)
                {
                    context.AddFailure("Não é permitido um co-executor destinar recursos a uma empresa Taesa");
                }

                if (alocacao.CoExecutorRecebedorId != null && alocacao.EmpresaRecebedoraId != null)
                {
                    context.AddFailure("Somente uma empresa recebedora do recurso por alocação");
                }

                if (alocacao.CoExecutorFinanciadorId != null && alocacao.EmpresaFinanciadoraId != null)
                {
                    context.AddFailure("Somente uma empresa financiadora do recurso por alocação");
                }
            });
        }
    }
}