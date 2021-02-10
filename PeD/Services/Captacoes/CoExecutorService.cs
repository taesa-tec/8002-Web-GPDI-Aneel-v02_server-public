using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Propostas;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class CoExecutorService : BaseService<CoExecutor>
    {
        public CoExecutorService(IRepository<CoExecutor> repository) : base(repository)
        {
        }

        public IList<CoExecutor> GetPorProposta(int propostaId) =>
            Filter(q => q.Where(ce => ce.PropostaId == propostaId));

        public IList<CoExecutor> GetPorResponsavel(string responsavelId) =>
            Filter(q => q
                .Include(ce => ce.Proposta)
                .ThenInclude(p => p.Fornecedor)
                .Where(ce => ce.Proposta.Fornecedor.ResponsavelId == responsavelId)
            );
    }
}