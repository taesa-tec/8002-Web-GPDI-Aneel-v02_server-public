using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Propostas;
using TaesaCore.Interfaces;
using TaesaCore.Services;

namespace PeD.Services.Captacoes
{
    public class EmpresaService : BaseService<Empresa>
    {
        public EmpresaService(IRepository<Empresa> repository) : base(repository)
        {
        }

        public IList<Empresa> GetPorProposta(int propostaId) =>
            Filter(q => q.Where(ce => ce.PropostaId == propostaId));

        public IList<Empresa> GetPorResponsavel(string responsavelId) =>
            Filter(q => q
                .Include(ce => ce.Proposta)
                .ThenInclude(p => p.Fornecedor)
                .Where(ce => ce.Proposta.Fornecedor.ResponsavelId == responsavelId)
            );
    }
}