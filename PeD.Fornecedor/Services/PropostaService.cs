using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Captacao;
using PeD.Data;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Fornecedor.Services
{
    public class PropostaService : BaseService<PropostaFornecedor>
    {
        private DbSet<PropostaFornecedor> _captacaoPropostas;

        public PropostaService(IRepository<PropostaFornecedor> repository, GestorDbContext context)
            : base(repository)
        {
            _captacaoPropostas = context.Set<PropostaFornecedor>();
        }

        public PropostaFornecedor GetProposta(int id)
        {
            return _captacaoPropostas
                .Include(p => p.Fornecedor)
                .Include(p => p.Captacao)
                .FirstOrDefault(p => p.Id == id);
        }


        public IEnumerable<PropostaFornecedor> GetPropostasPorResponsavel(string userId)
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