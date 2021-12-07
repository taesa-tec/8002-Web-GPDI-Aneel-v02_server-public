using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/RecursosMateriais/Alocacao")]
    public class
        AlocacaoRecursosMateriaisController : PropostaNodeBaseController<AlocacaoRm,
            AlocacaoRecursoMaterialRequest,
            AlocacaoRecursoMaterialDto>
    {
        private GestorDbContext _context;

        public AlocacaoRecursosMateriaisController(IService<AlocacaoRm> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper,
                authorizationService, propostaService)
        {
            _context = context;
        }

        protected override ActionResult Validate(AlocacaoRecursoMaterialRequest request)
        {
            if (!_context.Set<Empresa>()
                    .Any(e => e.Id == request.EmpresaFinanciadoraId && e.PropostaId == Proposta.Id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.EmpresaRecebedoraId && e.PropostaId == Proposta.Id))
                return ValidationProblem("Empresa Inválida");
            return null;
        }

        protected override IQueryable<AlocacaoRm> Includes(IQueryable<AlocacaoRm> queryable)
        {
            return queryable
                    .Include(r => r.Recurso)
                    .ThenInclude(r => r.CategoriaContabil)
                    .Include(r => r.Etapa)
                    .Include(r => r.EmpresaFinanciadora)
                    .Include(r => r.EmpresaRecebedora)
                ;
        }
    }
}