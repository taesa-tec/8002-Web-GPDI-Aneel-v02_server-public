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
    [Route("api/Propostas/{propostaId:guid}/RecursosMateriais/Alocacao")]
    public class
        AlocacaoRecursosMateriaisController : PropostaNodeBaseController<AlocacaoRm,
            AlocacaoRecursoMaterialRequest,
            AlocacaoRecursoMaterialDto>
    {
        public AlocacaoRecursosMateriaisController(IService<AlocacaoRm> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }

        protected override IQueryable<AlocacaoRm> Includes(
            IQueryable<AlocacaoRm> queryable)
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