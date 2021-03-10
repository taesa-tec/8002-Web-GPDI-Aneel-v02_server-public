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

            RuleFor(p => p.Aplicabilidade).NotEmpty().WithMessage("Aplicabilidade não informada");

            RuleFor(p => p.Bibliografia).NotEmpty().WithMessage("Bibliografia não informado");
            RuleFor(p => p.BuscaAnterioridade).NotEmpty().WithMessage("Busca de Anterioridade não informado");
            RuleFor(p => p.Motivacao).NotEmpty().WithMessage("Motivação não informado");
            RuleFor(p => p.PesquisasCorrelatasExecutora).NotEmpty()
                .WithMessage("Pesquisas Correlatas Executora não informado");
            RuleFor(p => p.PesquisasCorrelatasPeD).NotEmpty().WithMessage("Pesquisas Correlatas PeD não informado");
            RuleFor(p => p.PesquisasCorrelatasPeDAneel).NotEmpty()
                .WithMessage("Pesquisas Correlatas PeD ANEEL não informado");
            RuleFor(p => p.Originalidade).NotEmpty().WithMessage("Originalidade não informado");
            RuleFor(p => p.Relevancia).NotEmpty().WithMessage("Relevancia não informado");

            #endregion

            #region Escopo

            RuleFor(p => p.Objetivo).NotEmpty().WithMessage("Objetivo não informado");
            RuleFor(p => p.ResultadoEsperado).NotEmpty().WithMessage("Resultado Esperado não informado");
            RuleFor(p => p.Metas.Count).GreaterThan(1).WithMessage("Nenhuma meta do projeto cadastrada");
            RuleFor(p => p.BeneficioIndustria).NotEmpty().WithMessage("Benefício à industria não informado");
            RuleFor(p => p.BeneficioInstitucional).NotEmpty()
                .WithMessage("Benefícios à Instituição de Ensino/Pesquisa ou Empresa parceira não informado");
            RuleFor(p => p.BeneficioSociedade).NotEmpty().WithMessage("Benefícios à Sociedade não informado");
            RuleFor(p => p.BeneficioTaesa).NotEmpty().WithMessage("Benefícios à Taesa não informado");
            RuleFor(p => p.BeneficioSetorEletrico).NotEmpty().WithMessage("Benefícios ao Seto Eletrico não informado");
            RuleFor(p => p.Contrapartidas).NotEmpty().WithMessage("Contrapartidas não informado");
            RuleFor(p => p.ExperienciaPrevia).NotEmpty().WithMessage("Experiência Prévia não informado");

            #endregion

            RuleFor(p => p.Riscos.Count).GreaterThan(0).WithMessage("Nenhum Risco foi cadastrado");
            RuleFor(p => p.RecursosMateriaisAlocacoes.Count).GreaterThan(0)
                .WithMessage("Não há alocações de recursos Materiais");
            RuleFor(p => p.RecursosMateriais.Count).GreaterThan(0).WithMessage("Não há recursos Materiais cadastrados");
            RuleFor(p => p.RecursosHumanosAlocacoes.Count).GreaterThan(0)
                .WithMessage("Não há alocações de recursos humanos cadastrados");
            RuleFor(p => p.RecursosHumanos.Count).GreaterThan(0).WithMessage("Não há Recursos humanos cadastrados");
            RuleFor(p => p.Duracao).NotEmpty().GreaterThan((short) 0)
                .WithMessage("A duração do projeto não foi configurada");


            //Produtos
            RuleFor(p => p.Produtos).Custom((list, context) =>
            {
                if (!list.Exists(produto => produto.Classificacao == ProdutoClassificacao.Final))
                {
                    context.AddFailure("Produto Final não cadastrado");
                }
            });
            RuleFor(p => p.Etapas.Count).GreaterThan(0).WithMessage("Nenhuma Etapa cadastrada");
        }
    }
}