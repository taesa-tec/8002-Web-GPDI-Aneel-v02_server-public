using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using PeD.Data;
using PeD.Views.Email.Captacao;
using TaesaCore.Interfaces;
using TaesaCore.Services;
using Contrato = PeD.Core.Models.Contrato;

namespace PeD.Services.Captacoes
{
    public class CaptacaoService : BaseService<Captacao>
    {
        private SendGridService _sendGridService;
        private GestorDbContext _context;
        private DbSet<CaptacaoArquivo> _captacaoArquivos;
        private DbSet<CaptacaoFornecedor> _captacaoFornecedors;
        private DbSet<Proposta> _captacaoPropostas;

        public CaptacaoService(IRepository<Captacao> repository, GestorDbContext context,
            SendGridService sendGridService) : base(repository)
        {
            _context = context;
            _sendGridService = sendGridService;
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

        public async Task ConfigurarCaptacao(int id, DateTime termino, string consideracoes,
            IEnumerable<int> arquivosIds, IEnumerable<int> fornecedoresIds, int contratoId)
        {
            ThrowIfNotExist(id);
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

            await fornecedores.ForEachAsync(f =>
            {
                var convite = new ConviteFornecedor()
                {
                    Fornecedor = f.Nome,
                    Projeto = captacao.Titulo,
                    CaptacaoId = id
                };
                _sendGridService
                    .Send(f.Responsavel.Email,
                        "Você foi convidado para participar de um novo projeto para a área de P&D da Taesa",
                        "Email/Captacao/ConviteFornecedor",
                        convite)
                    .Wait();
            });
        }

        public async Task EstenderCaptacao(int id, DateTime termino)
        {
            ThrowIfNotExist(id);
            var captacao = Get(id);
            captacao.Termino = termino;
            Put(captacao);

            var fornecedores = _captacaoFornecedors
                .Include(cf => cf.Fornecedor)
                .ThenInclude(f => f.Responsavel)
                .Where(cf => cf.CaptacaoId == id)
                .Select(cf => cf.Fornecedor);

            await fornecedores.ForEachAsync(f =>
            {
                var cancelamento = new AlteracaoPrazo()
                {
                    Projeto = captacao.Titulo,
                    Prazo = termino,
                    CaptacaoId = captacao.Id
                };
                _sendGridService
                    .Send(f.Responsavel.Email,
                        $"A equipe de Suprimentos alterou a data máxima de envio de propostas para o projeto \"{captacao.Titulo}\".",
                        "Email/Captacao/AlteracaoPrazo",
                        cancelamento)
                    .Wait();
            });
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

            await fornecedores.ForEachAsync(f =>
            {
                var cancelamento = new CancelamentoCaptacao()
                {
                    Projeto = captacao.Titulo,
                };
                _sendGridService
                    .Send(f.Responsavel.Email,
                        $"A equipe de Suprimentos cancelou o processo de captação de propostas do projeto \"{captacao.Titulo}\".",
                        "Email/Captacao/CancelamentoCaptacao",
                        cancelamento)
                    .Wait();
            });
            // var fornecedores = _captacaoFornecedors.Include(cf => cf.Fornecedor).Where(cf => cf.CaptacaoId == id).Select(cf => cf.Fornecedor);
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
                .Where(cp => cp.CaptacaoId == captacaoId &&
                             cp.Participacao == StatusParticipacao.Aceito &&
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
                .Where(cp =>
                    cp.Fornecedor.ResponsavelId == userId &&
                    cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                    cp.Participacao != StatusParticipacao.Rejeitado)
                .ToList();
        }
    }
}