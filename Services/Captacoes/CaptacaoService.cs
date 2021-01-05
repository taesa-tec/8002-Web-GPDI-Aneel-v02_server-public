using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGestor.Data;
using APIGestor.Models.Captacao;
using Microsoft.EntityFrameworkCore;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace APIGestor.Services.Captacoes
{
    public class CaptacaoService : BaseService<Captacao>
    {
        private GestorDbContext _context;
        private DbSet<CaptacaoArquivo> _captacaoArquivos;
        private DbSet<CaptacaoFornecedor> _captacaoFornecedors;
        private DbSet<CaptacaoContrato> _captacaoContratos;
        private DbSet<PropostaFornecedor> _captacaoPropostas;

        public CaptacaoService(IRepository<Captacao> repository, GestorDbContext context) : base(repository)
        {
            _context = context;
            _captacaoArquivos = context.Set<CaptacaoArquivo>();
            _captacaoFornecedors = context.Set<CaptacaoFornecedor>();
            _captacaoContratos = context.Set<CaptacaoContrato>();
            _captacaoPropostas = context.Set<PropostaFornecedor>();
        }

        protected void ThrowIfNotExist(int id)
        {
            if (!Exist(id))
            {
                throw new Exception("Captação não encontrada");
            }
        }

        public async Task ConfigurarCaptacao(int id, DateTime termino, string consideracoes,
            IEnumerable<int> arquivosIds, IEnumerable<int> fornecedoresIds, IEnumerable<int> contratosIds)
        {
            ThrowIfNotExist(id);
            var captacao = Get(id);
            captacao.Termino = termino;
            captacao.Consideracoes = consideracoes;

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

            #region Contratos

            var contratosOld = _captacaoContratos.Where(c => c.CaptacaoId == id).ToList();
            _captacaoContratos.RemoveRange(contratosOld);
            _captacaoContratos.AddRange(contratosIds.Select(cid => new CaptacaoContrato()
            {
                CaptacaoId = id, ContratoId = cid
            }));

            #endregion

            await _context.SaveChangesAsync();
        }

        public async Task EnviarParaFornecedores(int id)
        {
            ThrowIfNotExist(id);

            var captacao = Get(id);
            captacao.Status = Captacao.CaptacaoStatus.Fornecedor;
            var fornecedores = _captacaoFornecedors.Include(cf => cf.Fornecedor).Where(cf => cf.CaptacaoId == id)
                .Select(cf => cf.Fornecedor);

            var propostas = fornecedores.Select(f => new PropostaFornecedor()
            {
                FornecedorId = f.Id,
                CaptacaoId = id,
                Participacao = PropostaFornecedor.StatusParticipacao.Pendente,
                Recebido = DateTime.Now
            });
            // @todo Enviar email para fornecedores
            _context.AddRange(propostas);
            _context.Update(captacao);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<PropostaFornecedor> GetPropostas(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Where(cp => cp.CaptacaoId == id)
                .ToList();
        }

        public IEnumerable<PropostaFornecedor> GetPropostas(int id, PropostaFornecedor.StatusParticipacao status)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Where(cp => cp.CaptacaoId == id && cp.Participacao == status)
                .ToList();
        }
    }
}