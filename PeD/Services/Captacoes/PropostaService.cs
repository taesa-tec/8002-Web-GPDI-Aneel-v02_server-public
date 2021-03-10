using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Core.Validators;
using PeD.Data;
using TaesaCore.Extensions;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class PropostaService : BaseService<Proposta>
    {
        private DbSet<Proposta> _captacaoPropostas;
        private DbSet<PropostaContrato> _propostasContratos;
        private IMapper _mapper;
        private IViewRenderService renderService;
        private GestorDbContext context;

        public PropostaService(IRepository<Proposta> repository, GestorDbContext context, IMapper mapper,
            IViewRenderService renderService)
            : base(repository)
        {
            this.context = context;
            _mapper = mapper;
            this.renderService = renderService;
            _captacaoPropostas = context.Set<Proposta>();
            _propostasContratos = context.Set<PropostaContrato>();
        }

        public Proposta GetProposta(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .FirstOrDefault(p => p.Id == id);
        }

        public Proposta GetPropostaFull(int id)
        {
            return _captacaoPropostas
                //Captacao
                .Include("Captacao.Tema")
                .Include(p => p.Captacao).ThenInclude(c => c.SubTemas).ThenInclude(s => s.SubTema)
                //

                // Produto
                .Include("Produtos.ProdutoTipo")
                .Include("Produtos.FaseCadeia")
                .Include("Produtos.TipoDetalhado")
                .Include("Etapas.Produto.ProdutoTipo")
                .Include("Etapas.Produto.FaseCadeia")
                .Include("Etapas.Produto.TipoDetalhado")
                // RH
                .Include(p => p.RecursosHumanos)
                .Include(p => p.RecursosHumanosAlocacoes)
                .Include("Etapas.RecursosHumanosAlocacoes.Recurso")
                .Include("Etapas.RecursosHumanosAlocacoes.EmpresaFinanciadora")
                .Include("Etapas.RecursosHumanosAlocacoes.CoExecutorFinanciador")
                // RM
                .Include(p => p.RecursosMateriais)
                .Include(p => p.RecursosMateriaisAlocacoes)
                .Include("Etapas.RecursosMateriaisAlocacoes.Recurso.CategoriaContabil")
                .Include("Etapas.RecursosMateriaisAlocacoes.EmpresaFinanciadora")
                .Include("Etapas.RecursosMateriaisAlocacoes.CoExecutorFinanciador")
                .Include("Etapas.RecursosMateriaisAlocacoes.EmpresaRecebedora")
                .Include("Etapas.RecursosMateriaisAlocacoes.CoExecutorRecebedor")
                .Include(p => p.CoExecutores)
                .Include(p => p.Escopo)
                .Include(p => p.Fornecedor)
                .Include(p => p.Metas)
                .Include(p => p.PlanoTrabalho)
                .Include(p => p.Produtos)
                .ThenInclude(p => p.FaseCadeia)
                .Include(p => p.Riscos)
                .FirstOrDefault(p => p.Id == id);
        }


        public Proposta GetPropostaPorFornecedor(int captacaoId, int fornecedorId) =>
            _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p =>
                    p.Captacao)
                .FirstOrDefault(cp => cp.CaptacaoId == captacaoId &&
                                      cp.Fornecedor.Id == fornecedorId &&
                                      cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                                      cp.Participacao != StatusParticipacao.Rejeitado);

        public IEnumerable<Proposta> GetPropostasPorResponsavel(string userId)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Where(cp =>
                    cp.Fornecedor.ResponsavelId == userId &&
                    cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                    cp.Participacao != StatusParticipacao.Rejeitado)
                .ToList();
        }

        public Proposta GetPropostaPorResponsavel(int captacaoId, string userId) =>
            _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Arquivos)
                .FirstOrDefault(cp => cp.Fornecedor.ResponsavelId == userId &&
                                      cp.CaptacaoId == captacaoId &&
                                      cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                                      cp.Participacao != StatusParticipacao.Rejeitado);

        public PropostaContrato GetContrato(int captacaoId, string userId)
        {
            var proposta = _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Contrato)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(cp => cp.Fornecedor.ResponsavelId == userId &&
                                      cp.CaptacaoId == captacaoId &&
                                      cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                                      cp.Participacao != StatusParticipacao.Rejeitado);
            if (proposta != null)
                return proposta.Contrato ?? new PropostaContrato()
                {
                    PropostaId = proposta.Id,
                    Parent = proposta.Captacao.Contrato,
                    ParentId = (int) proposta.Captacao?.ContratoId,
                };

            return null;
        }

        public PropostaContrato GetContrato(int contratoId, int propostaId)
        {
            return _propostasContratos
                .Include(p => p.Parent)
                .FirstOrDefault(c => c.PropostaId == propostaId && c.ParentId == contratoId);
        }

        public List<PropostaContratoRevisao> GetContratoRevisoes(int contratoId, int propostaId)
        {
            return context.Set<PropostaContratoRevisao>()
                .Include(cr => cr.Parent)
                .ThenInclude(c => c.Parent)
                .Where(cr => cr.Parent.ParentId == contratoId && cr.PropostaId == propostaId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToList();
        }

        public PropostaContratoRevisao GetContratoRevisao(int contratoId, int propostaId, int id)
        {
            return context.Set<PropostaContratoRevisao>()
                .Include(cr => cr.Parent)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(cr => cr.Parent.ParentId == contratoId && cr.PropostaId == propostaId && cr.Id == id);
        }

        public void UpdatePropostaDataAlteracao(int propostaId, DateTime time)
        {
            var proposta = GetProposta(propostaId);
            proposta.DataAlteracao = time;
            context.Update(proposta);
            context.SaveChanges();
        }

        public void UpdatePropostaDataAlteracao(int propostaId)
        {
            UpdatePropostaDataAlteracao(propostaId, DateTime.Now);
        }


        public Relatorio GetRelatorio(int propostaId)
        {
            var proposta = _captacaoPropostas.Include(p => p.Relatorio).FirstOrDefault(p => p.Id == propostaId);
            if (proposta != null)
            {
                if (proposta.Relatorio != null && proposta.Relatorio.DataAlteracao < proposta.DataAlteracao)
                {
                    return UpdateRelatorio(propostaId);
                }

                return proposta.Relatorio ?? UpdateRelatorio(propostaId);
            }

            return null;
        }

        public Relatorio UpdateRelatorio(int propostaId)
        {
            var proposta = GetPropostaFull(propostaId);
            var modelView = _mapper.Map<Core.Models.Relatorios.Fornecedores.Proposta>(proposta);
            var validacao = (new PropostaValidator()).Validate(modelView);
            var content = renderService.RenderToStringAsync("Proposta/Proposta", modelView).Result;
            var relatorio = context.Set<Relatorio>().Where(r => r.Id == propostaId).FirstOrDefault() ?? new Relatorio()
            {
                Content = content,
                DataAlteracao = DateTime.Now,
                PropostaId = propostaId,
                Validacao = validacao
            };

            if (relatorio.Id == 0)
            {
                context.Add(relatorio);
            }
            else
            {
                relatorio.Content = content;
                relatorio.DataAlteracao = DateTime.Now;
                relatorio.Validacao = validacao;
                context.Update(relatorio);
            }

            context.SaveChanges();
            proposta.RelatorioId = relatorio.Id;
            context.Update(proposta);
            context.SaveChanges();
            return relatorio;
        }
    }
}