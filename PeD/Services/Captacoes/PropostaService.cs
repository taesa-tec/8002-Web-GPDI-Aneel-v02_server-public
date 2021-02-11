using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Propostas;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class PropostaService : BaseService<Proposta>
    {
        private DbSet<Proposta> _captacaoPropostas;
        private DbSet<Contrato> _propostasContratos;

        public PropostaService(IRepository<Proposta> repository, DbContext context)
            : base(repository)
        {
            _captacaoPropostas = context.Set<Proposta>();
            _propostasContratos = context.Set<Contrato>();
        }

        public Proposta GetProposta(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
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

        public IEnumerable<Contrato> GetContratos(int captacaoId, string userId)
        {
            var proposta = _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .Include(p => p.Contratos)
                .ThenInclude(c => c.Parent)
                .FirstOrDefault(cp => cp.Fornecedor.ResponsavelId == userId &&
                                      cp.CaptacaoId == captacaoId &&
                                      cp.Captacao.Status == Captacao.CaptacaoStatus.Fornecedor &&
                                      cp.Participacao != StatusParticipacao.Rejeitado);
            if (proposta != null)
                return proposta.Contratos;

            return new List<Contrato>();
        }

        public Contrato GetContrato(int contratoId, int propostaId)
        {
            return _propostasContratos
                .Include(p => p.Parent)
                .FirstOrDefault(c => c.PropostaId == propostaId && c.ParentId == contratoId);
        }
    }
}