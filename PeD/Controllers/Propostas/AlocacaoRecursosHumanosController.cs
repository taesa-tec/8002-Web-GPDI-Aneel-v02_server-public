using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
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
    [Route("api/Propostas/{propostaId:guid}/RecursosHumanos/Alocacao")]
    public class
        AlocacaoRecursosHumanosController : PropostaNodeBaseController<RecursoHumano.AlocacaoRh,
            AlocacaoRecursoHumanoRequest,
            AlocacaoRecursoHumanoDto>
    {
        private GestorDbContext _context;

        public AlocacaoRecursosHumanosController(IService<RecursoHumano.AlocacaoRh> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper,
                authorizationService, propostaService)
        {
            _context = context;
        }

        protected override IQueryable<RecursoHumano.AlocacaoRh> Includes(IQueryable<RecursoHumano.AlocacaoRh> queryable)
        {
            return queryable
                    .Include(r => r.Recurso)
                    .Include(r => r.Etapa)
                    .Include(r => r.EmpresaFinanciadora)
                    .Include(r => r.CoExecutorFinanciador)
                ;
        }

        protected override ActionResult Validate(AlocacaoRecursoHumanoRequest request)
        {
            var recurso = _context.Set<RecursoHumano>().FirstOrDefault(r => r.Id == request.RecursoId);
            if (recurso != null && recurso.EmpresaId != null && request.CoExecutorFinanciadorId != null)
                return ValidationProblem("Não é permitido um co-executor destinar recursos a uma empresa Taesa");
            return null;
        }
    }
}