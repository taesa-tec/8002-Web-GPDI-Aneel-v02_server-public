using FluentValidation;
using PeD.Core.Models.Propostas;
using Proposta = PeD.Core.Models.Relatorios.Fornecedores.Proposta;

namespace PeD.Core.Validators
{
    public class PropostaValidator : AbstractValidator<Proposta>
    {
        public PropostaValidator()
        {
            #region Plano de trabalho

            RuleFor(p => p.Aplicabilidade).NotEmpty();

            RuleFor(p => p.Bibliografia).NotEmpty();
            RuleFor(p => p.BuscaAnterioridade).NotEmpty();
            RuleFor(p => p.Motivacao).NotEmpty();
            RuleFor(p => p.PesquisasCorrelatasExecutora).NotEmpty();
            RuleFor(p => p.PesquisasCorrelatasPeD).NotEmpty();
            RuleFor(p => p.PesquisasCorrelatasPeDAneel).NotEmpty();
            RuleFor(p => p.Originalidade).NotEmpty();
            RuleFor(p => p.Relevancia).NotEmpty();

            #endregion

            #region Escopo

            RuleFor(p => p.Objetivo).NotEmpty();
            RuleFor(p => p.ResultadoEsperado).NotEmpty();
            RuleFor(p => p.Metas.Count).GreaterThan(1);
            RuleFor(p => p.BeneficioIndustria).NotEmpty();
            RuleFor(p => p.BeneficioInstitucional).NotEmpty();
            RuleFor(p => p.BeneficioSociedade).NotEmpty();
            RuleFor(p => p.BeneficioTaesa).NotEmpty();
            RuleFor(p => p.BeneficioSetorEletrico).NotEmpty();
            RuleFor(p => p.Contrapartidas).NotEmpty();
            RuleFor(p => p.ExperienciaPrevia).NotEmpty();

            #endregion

            RuleFor(p => p.Riscos.Count).GreaterThan(0);
            RuleFor(p => p.RecursosMateriaisAlocacoes.Count).GreaterThan(0);
            RuleFor(p => p.RecursosMateriais.Count).GreaterThan(0);
            RuleFor(p => p.RecursosHumanosAlocacoes.Count).GreaterThan(0);
            RuleFor(p => p.RecursosHumanos.Count).GreaterThan(0);
            RuleFor(p => p.Duracao).NotEmpty().GreaterThan((short) 0);


            //Produtos
            RuleFor(p => p.Produtos).Custom((list, context) =>
            {
                if (!list.Exists(produto => produto.Classificacao == ProdutoClassificacao.Final))
                {
                    context.AddFailure("Produto Final não cadastrado");
                }
            });
            RuleFor(p => p.Etapas.Count).GreaterThan(0);
        }
    }
}