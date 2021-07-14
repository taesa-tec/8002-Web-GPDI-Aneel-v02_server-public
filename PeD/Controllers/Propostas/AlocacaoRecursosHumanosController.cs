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
using Empresa = PeD.Core.Models.Propostas.Empresa;

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
                ;
        }

        protected override ActionResult Validate(AlocacaoRecursoHumanoRequest request)
        {
            var recurso = _context.Set<RecursoHumano>().Include(r => r.Empresa)
                .FirstOrDefault(r => r.Id == request.RecursoId);
            var empresa = _context.Set<Empresa>().FirstOrDefault(e => e.Id == request.EmpresaFinanciadoraId);
            if (recurso != null && recurso.Empresa.Funcao == Funcao.Cooperada && empresa is {Funcao: Funcao.Executora})
                return ValidationProblem(
                    "Não é permitido um co-executor/executor destinar recursos a uma empresa Proponente/Cooperada");
            return null;
        }
    }
}