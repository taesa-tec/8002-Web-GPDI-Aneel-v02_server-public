using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using iText.Html2pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using PeD.Core.Validators;
using PeD.Data;
using PeD.Views.Email.Captacao.Propostas;
using TaesaCore.Extensions;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class PropostaService : BaseService<Proposta>
    {
        private DbSet<Proposta> _captacaoPropostas;
        private IMapper _mapper;
        private ILogger<PropostaService> _logger;
        private IViewRenderService renderService;
        private GestorDbContext context;
        private SendGridService _sendGridService;
        private UserService _userService;
        private ArquivoService _arquivoService;

        public PropostaService(IRepository<Proposta> repository, GestorDbContext context, IMapper mapper,
            IViewRenderService renderService, SendGridService sendGridService, UserService userService,
            ILogger<PropostaService> logger, ArquivoService arquivoService)
            : base(repository)
        {
            this.context = context;
            _mapper = mapper;
            this.renderService = renderService;
            _sendGridService = sendGridService;
            _userService = userService;
            _logger = logger;
            _arquivoService = arquivoService;
            _captacaoPropostas = context.Set<Proposta>();
        }

        #region Lists

        public IEnumerable<Proposta> GetPropostasEncerradasPendentes()
        {
            return _captacaoPropostas
                .AsQueryable()
                .Include(p => p.Fornecedor)
                .ThenInclude(f => f.Responsavel)
                .Include(p => p.Captacao)
                .Where(p => p.Participacao == StatusParticipacao.Aceito
                            && p.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor
                            && p.Captacao.Termino < DateTime.Today).ToList();
        }


        public List<Proposta> GetPropostasEncerradas(string responsavelId) =>
            _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p =>
                    p.Captacao)
                .Where(cp => cp.ResponsavelId == responsavelId &&
                             (cp.Captacao.Status >= Captacao.CaptacaoStatus.Encerrada ||
                              cp.Participacao == StatusParticipacao.Rejeitado)).ToList();

        public IEnumerable<Proposta> GetPropostasPorResponsavel(string userId,
            Captacao.CaptacaoStatus status = Captacao.CaptacaoStatus.Fornecedor)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Include(p => p.Contrato)
                .Where(cp =>
                    cp.ResponsavelId == userId &&
                    cp.Captacao.Status == status &&
                    cp.Participacao != StatusParticipacao.Rejeitado)
                .ToList();
        }

        #endregion

        public Proposta GetProposta(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .FirstOrDefault(p => p.Id == id);
        }

        public Proposta GetProposta(Guid guid)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Contrato)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Arquivos)
                .FirstOrDefault(p => p.Guid == guid);
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

        public Proposta GetPropostaPorResponsavel(int captacaoId, string userId)
        {
            return GetPropostaPorResponsavel(captacaoId, userId, Captacao.CaptacaoStatus.Fornecedor);
        }

        public Proposta GetPropostaPorResponsavel(int captacaoId, string userId,
            params Captacao.CaptacaoStatus[] status)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Arquivos)
                .FirstOrDefault(cp => cp.Fornecedor.ResponsavelId == userId &&
                                      cp.CaptacaoId == captacaoId &&
                                      status.Contains(cp.Captacao.Status) &&
                                      cp.Participacao != StatusParticipacao.Rejeitado);
        }

        public PropostaContrato GetContrato(int propostaId)
        {
            var proposta = _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Contrato)
                .Include(p => p.Contrato).ThenInclude(c => c.File)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(c => c.Id == propostaId);


            if (proposta?.Captacao?.ContratoId != null)
                return proposta.Contrato ?? new PropostaContrato()
                {
                    PropostaId = proposta.Id,
                    Parent = proposta.Captacao.Contrato,
                    ParentId = (int) proposta.Captacao?.ContratoId,
                };

            return null;
        }

        public PropostaContrato GetContrato(Guid guid)
        {
            var proposta = _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Contrato)
                .Include(p => p.Contrato).ThenInclude(c => c.File)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(c => c.Guid == guid);


            if (proposta?.Captacao?.ContratoId != null)
                return proposta.Contrato ?? new PropostaContrato()
                {
                    PropostaId = proposta.Id,
                    Parent = proposta.Captacao.Contrato,
                    ParentId = (int) proposta.Captacao?.ContratoId,
                };

            return null;
        }

        public PropostaContrato GetContratoFull(int propostaId)
        {
            var proposta = _captacaoPropostas
                .AsNoTracking()
                .Include(p => p.Fornecedor)
                .Include(p => p.Produtos)
                .ThenInclude(p => p.FaseCadeia)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Contrato)
                .Include(p => p.Captacao)
                .ThenInclude(c => c.Tema)
                .Include(p => p.Contrato)
                .ThenInclude(c => c.Parent)
                .Include(p => p.Etapas)
                .ThenInclude(e => e.RecursosHumanosAlocacoes)
                .ThenInclude(r => r.Recurso)
                .Include(p => p.Etapas)
                .ThenInclude(e => e.RecursosMateriaisAlocacoes)
                .ThenInclude(r => r.Recurso)
                .FirstOrDefault(c => c.Id == propostaId);
            if (proposta?.Captacao?.ContratoId != null)
                return proposta.Contrato ?? new PropostaContrato()
                {
                    PropostaId = proposta.Id,
                    Parent = proposta.Captacao.Contrato,
                    ParentId = (int) proposta.Captacao?.ContratoId,
                };
            return null;
        }

        public string PrintContrato(int propostaId)
        {
            var contrato = GetContratoFull(propostaId);
            _logger.LogInformation("Total de produtos {Total}", contrato.Proposta.Produtos.Count);
            var contratoDto = _mapper.Map<PropostaContratoDto>(contrato);
            return renderService.RenderToStringAsync("Proposta/Contrato", contratoDto).Result;
        }

        public List<PropostaContratoRevisao> GetContratoRevisoes(int propostaId)
        {
            return context.Set<PropostaContratoRevisao>()
                .Include(cr => cr.Parent)
                .ThenInclude(c => c.Parent)
                .Where(cr => cr.PropostaId == propostaId)
                .OrderByDescending(cr => cr.CreatedAt)
                .ToList();
        }

        public PropostaContratoRevisao GetContratoRevisao(int propostaId, int id)
        {
            return context.Set<PropostaContratoRevisao>()
                .Include(cr => cr.Parent)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(cr => cr.PropostaId == propostaId && cr.Id == id);
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

        public async Task FinalizarProposta(int propostaId)
        {
            await FinalizarProposta(GetProposta(propostaId));
        }

        public async Task FinalizarProposta(Proposta proposta)
        {
            if (proposta.Participacao == StatusParticipacao.Aceito)
            {
                proposta.Finalizado = true;
                proposta.DataResposta = DateTime.Now;
                Put(proposta);
                await SendEmailFinalizado(proposta);
            }
        }

        public async Task FinalizarPropostasExpiradas(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Finalizando Propostas expiradas");
            if (stoppingToken.IsCancellationRequested)
                return;
            var propostas = GetPropostasEncerradasPendentes();
            _logger.LogInformation($"Propostas expiradas: {propostas.Count()}");
            foreach (var proposta in propostas)
            {
                proposta.Participacao = StatusParticipacao.Concluido;
                Put(proposta);
                await SendEmailCaptacaoEncerrada(proposta);
            }
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
                context.SaveChanges();
            }
            else
            {
                relatorio.Content = content;
                relatorio.DataAlteracao = DateTime.Now;
                relatorio.Validacao = validacao;
                context.Update(relatorio);
                context.SaveChanges();
            }

            var file = SaveRelatorioPdf(relatorio);
            relatorio.File = file;
            relatorio.FileId = file.Id;
            proposta.RelatorioId = relatorio.Id;
            context.Update(proposta);
            context.SaveChanges();
            return relatorio;
        }

        public Relatorio GetRelatorio(int propostaId)
        {
            var proposta = _captacaoPropostas.Include(p => p.Relatorio).FirstOrDefault(p => p.Id == propostaId);
            if (proposta != null)
            {
                if (proposta.Relatorio == null || proposta.Relatorio.DataAlteracao < proposta.DataAlteracao)
                {
                    return UpdateRelatorio(propostaId);
                }

                return proposta.Relatorio;
            }

            return null;
        }


        public FileUpload SaveRelatorioPdf(Relatorio relatorio)
        {
            if (relatorio != null)
            {
                var file = Path.GetTempFileName();
                var stream = new FileStream(file, FileMode.Create);
                HtmlConverter.ConvertToPdf(relatorio.Content, stream);
                stream.Close();

                var arquivo = _arquivoService.FromPath(file, "application/pdf",
                    $"relatorio-{relatorio.PropostaId}-{relatorio.DataAlteracao}.pdf");

                return arquivo;
            }

            return null;
        }

        public FileUpload GetRelatorioPdf(int propostaId)
        {
            var proposta = _captacaoPropostas.AsQueryable().Include(p => p.Relatorio)
                .ThenInclude(r => r.File)
                .FirstOrDefault(p => p.Id == propostaId);
            if (proposta != null && proposta?.Relatorio.File != null)
            {
                return proposta.Relatorio.File;
            }

            return null;
        }


        public FileUpload SaveContratoPdf(PropostaContrato contrato)
        {
            var contratoContent = PrintContrato(contrato.PropostaId);
            if (contratoContent != null)
            {
                var file = Path.GetTempFileName();
                var stream = new FileStream(file, FileMode.Create);
                HtmlConverter.ConvertToPdf(contratoContent, stream);
                stream.Close();
                PdfHelper.AddPagesToPdf(file, 475, 90);
                var arquivo = _arquivoService.FromPath(file, "application/pdf",
                    $"contrato-{contrato.PropostaId}.pdf");
                return arquivo;
            }

            return null;
        }

        public FileUpload GetContratoPdf(int propostaId)
        {
            var contrato = GetContrato(propostaId);
            return contrato?.File;
        }

        #region Emails

        public async Task SendEmailFinalizado(Proposta proposta)
        {
            var pf = _mapper.Map<PropostaFinalizada>(proposta);
            var suprimentoUsers = _userService.GetInRole("Suprimento").Select(u => u.Email);
            var subject = pf.Cancelada
                ? $"O fornecedor “{pf.Fornecedor}” cancelou sua participação no projeto"
                : $"O fornecedor “{pf.Fornecedor}” finalizou com sucesso a sua participação no projeto";
            await _sendGridService.Send(suprimentoUsers,
                subject,
                "Email/Captacao/Propostas/PropostaFinalizada", pf);
        }

        public async Task SendEmailCaptacaoEncerrada(Proposta proposta)
        {
            var pf = _mapper.Map<PropostaFinalizada>(proposta);

            var subject = pf.Finalizado
                ? $"Sua participação no projeto “{pf.Projeto}” foi concluída com sucesso."
                : $"Os itens do projeto “{pf.Projeto}” não foram enviados até a data máxima.";
            await _sendGridService.Send(proposta.Fornecedor.Responsavel.Email,
                subject,
                "Email/Captacao/Propostas/PropostaEncerrada", pf);
        }

        #endregion
    }
}