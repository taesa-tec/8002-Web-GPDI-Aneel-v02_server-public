using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClosedXML.Excel.CalcEngine.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeD.Core.Exceptions.Captacoes;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Views.Email.Captacao;
using PeD.Views.Email.Captacao.Propostas;
using TaesaCore.Interfaces;
using TaesaCore.Models;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class CaptacaoService : BaseService<Captacao>
    {
        private ILogger<CaptacaoService> _logger;
        private SendGridService _sendGridService;
        private GestorDbContext _context;
        private DbSet<CaptacaoArquivo> _captacaoArquivos;
        private DbSet<CaptacaoFornecedor> _captacaoFornecedors;
        private DbSet<Proposta> _captacaoPropostas;
        private PropostaService _propostaService;
        private IMapper _mapper;

        public CaptacaoService(IRepository<Captacao> repository, GestorDbContext context,
            SendGridService sendGridService, ILogger<CaptacaoService> logger, PropostaService propostaService,
            IMapper mapper) : base(repository)
        {
            _context = context;
            _sendGridService = sendGridService;
            _logger = logger;
            _propostaService = propostaService;
            _mapper = mapper;
            _captacaoArquivos = context.Set<CaptacaoArquivo>();
            _captacaoFornecedors = context.Set<CaptacaoFornecedor>();
            _captacaoPropostas = context.Set<Proposta>();
        }

        protected void ThrowIfNotExist(int id)
        {
            if (!Exist(id))
            {
                throw new Exception("Captação não encontrada");
            }
        }

        public string UserSuprimento(int id) => _context.Set<Captacao>().Where(c => c.Id == id)
            .Select(c => c.UsuarioSuprimentoId).FirstOrDefault();

        public List<Captacao> GetCaptacoes(Captacao.CaptacaoStatus status)
        {
            return Filter(q => q
                    .Include(c => c.Criador)
                    .Include(c => c.UsuarioSuprimento)
                    .Include(c => c.Propostas)
                    .ThenInclude(p => p.Fornecedor)
                    .Where(c => c.Status == status))
                .ToList();
        }

        public List<Captacao> GetCaptacoesFalhas()
        {
            return Filter(q => q
                    .Include(c => c.Criador)
                    .Include(c => c.UsuarioSuprimento)
                    .Include(c => c.Propostas)
                    .Where(c => c.Status == Captacao.CaptacaoStatus.Cancelada ||
                                c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                                (c.Propostas.Count == 0 ||
                                 c.Propostas.Any(p => !p.Finalizado || !p.Contrato.Finalizado))
                    ))
                //@todo Verificar se funciona propostas zeradas
                .ToList();
        }

        public List<Captacao> GetCaptacoesEncerradas()
        {
            return Filter(q => q
                    .Include(c => c.Criador)
                    .Include(c => c.UsuarioSuprimento)
                    .Include(c => c.Propostas)
                    .ThenInclude(p => p.Fornecedor)
                    .Include(c => c.Propostas)
                    .ThenInclude(p => p.Contrato)
                    .Where(c => (c.Status >= Captacao.CaptacaoStatus.Encerrada || c.Termino < DateTime.Today)
                                && c.Propostas.Any(p => p.Finalizado && p.Contrato.Finalizado)
                    ))
                .ToList();
        }

        public List<Captacao> GetCaptacoesSelecaoPendente()
        {
            var captacoes = _context.Set<Captacao>().AsQueryable();
            var propostas = _context.Set<Proposta>().AsQueryable();
            var contratos = _context.Set<PropostaContrato>().AsQueryable();
            var pendentes =
                from captacao in captacoes
                join proposta in propostas on captacao.Id equals proposta.CaptacaoId
                join contrato in contratos on proposta.Id equals contrato.PropostaId
                where captacao.Status == Captacao.CaptacaoStatus.Encerrada
                      && captacao.PropostaSelecionadaId == null
                      && proposta.Finalizado
                      && contrato.Finalizado
                select captacao;
            return pendentes.Include(c => c.Propostas)
                .ThenInclude(p => p.Contrato)
                .ToList();
        }

        public List<Captacao> GetCaptacoesSelecaoFinalizada()
        {
            return Filter(q => q
                    .Include(c => c.Propostas)
                    .ThenInclude(p => p.Fornecedor)
                    .Include(c => c.UsuarioRefinamento)
                    .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada && c.PropostaSelecionadaId != null))
                .ToList();
        }

        public List<Captacao> GetCaptacoesPorSuprimento(string userId)
        {
            var captacoesQuery =
                from captacao in _context.Set<Captacao>().AsQueryable()
                where
                    captacao.UsuarioSuprimentoId == userId &&
                    (
                        captacao.Status == Captacao.CaptacaoStatus.Elaboracao ||
                        captacao.Status == Captacao.CaptacaoStatus.Fornecedor ||
                        captacao.Status == Captacao.CaptacaoStatus.Encerrada
                    )
                select captacao;


            return captacoesQuery
                .Include(c => c.UsuarioSuprimento)
                .ToList();
        }

        public List<Captacao> GetCaptacoesPorSuprimento(string userId, Captacao.CaptacaoStatus status)
        {
            var captacoesQuery =
                from captacao in _context.Set<Captacao>().AsQueryable()
                where
                    captacao.UsuarioSuprimentoId == userId && captacao.Status == status
                select captacao;

            return captacoesQuery
                .Include(c => c.UsuarioSuprimento)
                .ToList();
        }

        public async Task ConfigurarCaptacao(int id, DateTime termino, string consideracoes,
            IEnumerable<int> arquivosIds, IEnumerable<int> fornecedoresIds, int contratoId)
        {
            ThrowIfNotExist(id);
            if (termino < DateTime.Today)
            {
                throw new CaptacaoException("A data máxima não pode ser anterior a data de hoje");
            }

            var captacao = Get(id);
            captacao.Termino = termino;
            captacao.Consideracoes = consideracoes;
            captacao.ContratoId = contratoId;

            #region Arquivos

            var arquivos = _captacaoArquivos.Where(ca => ca.CaptacaoId == id).ToList();
            arquivos.ForEach(arquivo => arquivo.AcessoFornecedor = arquivosIds.Contains(arquivo.Id));
            _captacaoArquivos.UpdateRange(arquivos);

            #endregion

            #region Fornecedores

            var fornecedores = fornecedoresIds.Select(fid => new CaptacaoFornecedor
                {CaptacaoId = id, FornecedorId = fid});

            var captacaoFornecedors = _captacaoFornecedors.Where(f => f.CaptacaoId == id).ToList();
            _captacaoFornecedors.RemoveRange(captacaoFornecedors);
            _captacaoFornecedors.AddRange(fornecedores);

            #endregion

            await _context.SaveChangesAsync();
        }


        public async Task EnviarParaFornecedores(int id)
        {
            ThrowIfNotExist(id);

            var captacao = Get(id);
            captacao.Status = Captacao.CaptacaoStatus.Fornecedor;
            var fornecedores = _captacaoFornecedors
                .Include(cf => cf.Fornecedor)
                .ThenInclude(f => f.Responsavel)
                .Where(cf => cf.CaptacaoId == id)
                .Select(cf => cf.Fornecedor);

            var propostas = fornecedores.Select(f => new Proposta()
            {
                FornecedorId = f.Id,
                Fornecedor = f,
                ResponsavelId = f.ResponsavelId,
                CaptacaoId = id,
                Contrato = new PropostaContrato()
                {
                    ParentId = (int) captacao.ContratoId
                },
                Participacao = StatusParticipacao.Pendente,
                DataCriacao = DateTime.Now
            });

            _context.AddRange(propostas);
            _context.Update(captacao);
            await _context.SaveChangesAsync();

            await SendEmailConvite(captacao);
        }

        public async Task EstenderCaptacao(int id, DateTime termino)
        {
            ThrowIfNotExist(id);
            var captacao = Get(id);
            if (termino < DateTime.Today || termino < captacao.Termino)
            {
                throw new CaptacaoException("A data máxima não pode ser anterior a data previamente escolhida");
            }

            captacao.Termino = termino;
            Put(captacao);

            try
            {
                await SendEmailAtualizacao(captacao);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public async Task CancelarCaptacao(int id)
        {
            ThrowIfNotExist(id);
            var captacao = Get(id);
            captacao.Status = Captacao.CaptacaoStatus.Cancelada;
            captacao.Cancelamento = DateTime.Now;
            Put(captacao);

            var fornecedores = _captacaoFornecedors
                .Include(cf => cf.Fornecedor)
                .ThenInclude(f => f.Responsavel)
                .Where(cf => cf.CaptacaoId == id)
                .Select(cf => cf.Fornecedor);
            await SendEmailCancelamento(captacao, fornecedores.ToList());
        }

        public Proposta GetProposta(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Include(p => p.Contrato)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Proposta> GetPropostasPorCaptacao(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Where(cp => cp.CaptacaoId == id)
                .ToList();
        }

        public IEnumerable<Proposta> GetPropostasPorCaptacao(int id, StatusParticipacao status)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Where(cp => cp.CaptacaoId == id && cp.Participacao == status)
                .ToList();
        }

        public IEnumerable<Proposta> GetPropostasEmAberto(int captacaoId)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Contrato)
                .Include(p => p.Captacao)
                .Where(cp => cp.CaptacaoId == captacaoId && cp.Participacao != StatusParticipacao.Rejeitado &&
                             (!cp.Finalizado || (cp.Contrato != null && !cp.Contrato.Finalizado))
                )
                .ToList();
        }

        public IEnumerable<Proposta> GetPropostasRecebidas(int captacaoId)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Contrato)
                .Include(p => p.Captacao)
                .Where(cp => cp.CaptacaoId == captacaoId &&
                             (cp.Participacao == StatusParticipacao.Aceito ||
                              cp.Participacao == StatusParticipacao.Concluido) &&
                             cp.Finalizado && cp.Contrato != null &&
                             cp.Contrato.Finalizado
                )
                .ToList();
        }

        public IEnumerable<Proposta> GetPropostasPorResponsavel(string userId)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Include(p => p.Captacao)
                .Where(cp =>
                    cp.ResponsavelId == userId &&
                    cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                    cp.Participacao != StatusParticipacao.Rejeitado)
                .ToList();
        }

        public void EncerrarCaptacoesExpiradas()
        {
            var expiradas = Filter(q =>
                    q.Include(c => c.Propostas)
                        .ThenInclude(p => p.Contrato)
                        .Where(c => c.Status == Captacao.CaptacaoStatus.Fornecedor && c.Termino < DateTime.Today))
                .ToList();
            foreach (var expirada in expiradas)
            {
                var isOk = expirada.Propostas.Count > 0 &&
                           expirada.Propostas.Any(p => p.Finalizado && p.Contrato.Finalizado);
                expirada.Status = isOk ? Captacao.CaptacaoStatus.Encerrada : Captacao.CaptacaoStatus.Cancelada;
                if (expirada.Status == Captacao.CaptacaoStatus.Cancelada)
                {
                    expirada.Cancelamento = DateTime.Now;
                }
            }

            Put(expiradas);
            _logger.LogInformation("Captações expiradas: {Count}", expiradas.Count);
        }

        public IEnumerable<Proposta> GetPropostasRefinamento(string userId, bool asFornecedor = false)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Include(p => p.Captacao)
                .Where(proposta =>
                    proposta.Id == proposta.Captacao.PropostaSelecionadaId &&
                    (
                        string.IsNullOrEmpty(userId) ||
                        asFornecedor && proposta.ResponsavelId == userId ||
                        proposta.Captacao.UsuarioRefinamentoId == userId
                    )
                    && proposta.Captacao.Status == Captacao.CaptacaoStatus.Refinamento &&
                    (proposta.ContratoAprovacao != StatusAprovacao.Aprovado ||
                     proposta.PlanoTrabalhoAprovacao != StatusAprovacao.Aprovado)
                )
                .ToList();
        }

        #region 2.5

        public List<Captacao> GetIdentificaoRiscoPendente()
        {
            var captacoes = _context.Set<Captacao>().AsQueryable();
            var pendentes =
                from captacao in captacoes
                where captacao.Status == Captacao.CaptacaoStatus.AnaliseRisco
                      && captacao.PropostaSelecionadaId != null
                      && (captacao.UsuarioAprovacaoId == null || captacao.ArquivoRiscosId == null)
                select captacao;

            return pendentes
                .Include(c => c.PropostaSelecionada)
                .ThenInclude(p => p.Fornecedor)
                .Include(c => c.UsuarioRefinamento)
                .ToList();
        }

        public List<Captacao> GetIdentificaoRiscoFinalizada()
        {
            var captacoes = _context.Set<Captacao>().AsQueryable();
            var finalizados =
                from captacao in captacoes
                where (captacao.Status == Captacao.CaptacaoStatus.AnaliseRisco ||
                       captacao.Status == Captacao.CaptacaoStatus.Formalizacao)
                      && captacao.PropostaSelecionadaId != null
                      && (captacao.UsuarioAprovacaoId != null || captacao.ArquivoRiscosId != null)
                select captacao;

            return finalizados
                .Include(c => c.PropostaSelecionada)
                .ThenInclude(p => p.Fornecedor)
                .Include(c => c.UsuarioAprovacao)
                .ToList();
        }

        #endregion

        #region 2.6

        public List<Captacao> GetFormalizacao(bool? formalizacao)
        {
            var captacoes = _context.Set<Captacao>().AsQueryable();
            var captacoesQuery =
                from captacao in captacoes
                where captacao.Status == Captacao.CaptacaoStatus.Formalizacao
                      && captacao.PropostaSelecionadaId != null
                      && captacao.IsProjetoAprovado == formalizacao
                select captacao;

            return captacoesQuery
                .Include(c => c.PropostaSelecionada)
                .ThenInclude(p => p.Fornecedor)
                .Include(c => c.UsuarioAprovacao)
                .Include(c => c.UsuarioExecucao)
                .ToList();
        }

        public Projeto CriarProjeto(Captacao captacao)
        {
            if (captacao == null || captacao.PropostaSelecionadaId == null)
                throw new NullReferenceException();
            if (captacao.IsProjetoAprovado == null || !captacao.IsProjetoAprovado.Value)
                throw new CaptacaoException("Projeto não foi aprovado");

            var proposta = _propostaService.GetPropostaFull(captacao.PropostaSelecionadaId.Value);
            _context.Entry(proposta).State = EntityState.Detached;

            var projeto = _mapper.Map<Projeto>(proposta);
            projeto.Produtos.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.CoExecutores.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.Riscos.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.RecursosHumanos.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.RecursosMateriais.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.Etapas.ForEach(e => { e.Id = 0; e.ProjetoId = 0; });
            projeto.RecursosHumanosAlocacoes.ForEach(e => { e.Id = 0; e.ProjetoId = 0; e.RecursoId = 0; });
            projeto.RecursosMateriaisAlocacoes.ForEach(e => { e.Id = 0; e.ProjetoId = 0;e.RecursoId = 0; });


            _context.Add(projeto);
            _context.SaveChanges();
            return projeto;
        }

        #endregion


        #region Emails

        public async Task SendEmailSuprimento(Captacao captacao, string autor)
        {
            var nova = new NovaCaptacao()
            {
                Autor = autor,
                CaptacaoId = captacao.Id,
                CaptacaoTitulo = captacao.Titulo
            };
            var email = _context.Users.AsQueryable()
                .Where(u => u.Role == "Suprimento" && u.Id == captacao.UsuarioSuprimentoId)
                .Select(u => u.Email)
                .FirstOrDefault();
            await _sendGridService.Send(email, "Novo Projeto para Captação de Proposta no Mercado cadastrado",
                "Email/Captacao/NovaCaptacao", nova);
        }

        public async Task SendEmailConvite(Captacao captacao)
        {
            var propostas = _context.Set<Proposta>().AsQueryable().AsNoTracking()
                .Include(p => p.Fornecedor)
                .Include(p => p.Responsavel)
                .Where(p => p.CaptacaoId == captacao.Id);
            foreach (var proposta in propostas)
            {
                var convite = new ConviteFornecedor()
                {
                    Fornecedor = proposta.Fornecedor.Nome,
                    Projeto = captacao.Titulo,
                    PropostaGuid = proposta.Guid
                };
                
                if (proposta.Responsavel == null ||
                    string.IsNullOrWhiteSpace(proposta.Responsavel.Email))
                {
                    _logger.LogWarning("Fornecedor como responsável|email vazio, não será possível enviar o convite");
                }
                else
                {
                    await _sendGridService
                        .Send(proposta.Responsavel.Email,
                            "Você foi convidado para participar de um novo projeto para a área de P&D da Taesa",
                            "Email/Captacao/ConviteFornecedor",
                            convite);
                }
            }
        }

        public async Task SendEmailAtualizacao(Captacao captacao)
        {
            var propostas = _context.Set<Captacao>().AsQueryable()
                .Include(c => c.Propostas)
                .ThenInclude(p => p.Responsavel)
                .First(c => c.Id == captacao.Id).Propostas;


            if (captacao.Termino != null)
            {
                foreach (var proposta in propostas)
                {
                    if (proposta.Responsavel == null)
                        continue;
                    var cancelamento = new AlteracaoPrazo()
                    {
                        Projeto = captacao.Titulo,
                        Prazo = captacao.Termino.Value,
                        PropostaGuid = proposta.Guid
                    };
                    await _sendGridService
                        .Send(proposta.Responsavel.Email,
                            $"A equipe de Suprimentos alterou a data máxima de envio de propostas para o projeto \"{captacao.Titulo}\".",
                            "Email/Captacao/AlteracaoPrazo",
                            cancelamento);
                }
            }
        }

        public async Task SendEmailCancelamento(Captacao captacao, List<Fornecedor> fornecedores)
        {
            var emails = fornecedores.Select(f => f.Responsavel.Email);
            var cancelamento = new CancelamentoCaptacao()
            {
                Projeto = captacao.Titulo,
            };
            await _sendGridService.Send(emails,
                $"A equipe de Suprimentos cancelou o processo de captação de propostas do projeto \"{captacao.Titulo}\".",
                "Email/Captacao/CancelamentoCaptacao",
                cancelamento);
        }

        public async Task SendEmailSelecao(Captacao captacao)
        {
            var emailRevisor = _context.Users.AsQueryable()
                .Where(u => u.Id == captacao.UsuarioRefinamentoId)
                .Select(u => u.Email)
                .FirstOrDefault();
            var proposta = _context.Set<Proposta>()
                .Include(p => p.Fornecedor)
                .Include(f => f.Responsavel)
                .FirstOrDefault(p => p.Id == captacao.PropostaSelecionadaId) ?? throw new NullReferenceException();
            var fornecedor = proposta.Fornecedor.Nome;


            var convite = new RevisorConvite()
            {
                Captacao = captacao,
                Fornecedor = fornecedor,
                PropostaGuid = proposta.Guid,
                DataAlvo = captacao.DataAlvo ?? throw new NullReferenceException()
            };
            var propostaSelecionada = new PropostaSelecionada()
            {
                Captacao = captacao,
                DataAlvo = convite.DataAlvo,
                PropostaGuid = proposta.Guid
            };

            await _sendGridService.Send(emailRevisor,
                "Você foi convidado a participar da Etapa de Refinamento da Proposta",
                "Email/Captacao/Propostas/RevisorConvite", convite);

            await _sendGridService.Send(proposta.Responsavel.Email,
                "Parabéns, sua proposta foi aprovada na Etapa de Priorização e Seleção",
                "Email/Captacao/Propostas/PropostaSelecionada", propostaSelecionada);
        }

        public async Task SendEmailFormalizacaoPendente(Captacao captacao)
        {
            var email = _context.Users.AsQueryable()
                .Where(u => u.Id == captacao.UsuarioAprovacaoId)
                .Select(u => u.Email)
                .FirstOrDefault() ?? throw new NullReferenceException();

            var formalizacao = new Formalizacao()
            {
                Captacao = captacao
            };
            await _sendGridService.Send(email,
                "Existe um novo projeto preparado para Aprovação e Formalização",
                "Email/Captacao/Formalizacao", formalizacao);
        }

        #endregion
    }
}