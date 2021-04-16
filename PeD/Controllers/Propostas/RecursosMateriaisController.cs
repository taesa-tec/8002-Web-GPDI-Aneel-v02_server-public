using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class RecursosMateriaisController : PropostaNodeBaseController<RecursoMaterial, RecursoMaterialRequest,
        RecursoMaterialDto>
    {
        private GestorDbContext _context;

        public RecursosMateriaisController(IService<RecursoMaterial> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }

        protected override IQueryable<RecursoMaterial> Includes(IQueryable<RecursoMaterial> queryable)
        {
            return queryable
                    .Include(r => r.CategoriaContabil)
                ;
        }

        public override async Task<IActionResult> Delete(int id)
        {
            if (_context.Set<RecursoMaterial.AlocacaoRm>().AsQueryable().Any(a => a.RecursoId == id))
            {
                return Problem("Não é possível apagar um recurso já alocado", null, StatusCodes.Status409Conflict);
            }

            return await base.Delete(id);
        }
    }
}