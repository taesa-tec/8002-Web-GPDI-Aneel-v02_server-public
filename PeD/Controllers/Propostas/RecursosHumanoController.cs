using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class
        RecursosHumanoController : PropostaNodeBaseController<RecursoHumano, RecursoHumanoRequest, RecursoHumanoDto>
    {
        private GestorDbContext _context;

        public RecursosHumanoController(IService<RecursoHumano> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper,
                authorizationService, propostaService)
        {
            _context = context;
        }

        protected override IQueryable<RecursoHumano> Includes(IQueryable<RecursoHumano> queryable)
        {
            return queryable
                    .Include(r => r.Empresa)
                ;
        }

        public override async Task<IActionResult> Delete(int id)
        {
            if (_context.Set<AlocacaoRh>().AsQueryable().Any(a => a.RecursoId == id))
            {
                return Problem("Não é possível apagar um recurso já alocado", null, StatusCodes.Status409Conflict);
            }

            return await base.Delete(id);
        }
    }
}