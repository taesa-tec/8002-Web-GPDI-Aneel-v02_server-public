using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
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
        public AlocacaoRecursosHumanosController(IService<RecursoHumano.AlocacaoRh> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
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
    }
}