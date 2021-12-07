using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Authorizations;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Projetos;
using PeD.Core.Requests.Projetos;
using PeD.Data;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/Recursos/Materiais")]
    public class RecursosMateriaisController : ControllerServiceBase<RecursoMaterial>
    {
        private GestorDbContext _context;

        public RecursosMateriaisController(IService<RecursoMaterial> service, IMapper mapper, GestorDbContext context) :
            base(service, mapper)
        {
            _context = context;
        }

        protected async Task<bool> HasAccess(int projetoId)
        {
            var projeto = _context.Set<Projeto>().Find(projetoId);
            if (User.IsInRole(Roles.Administrador) || this.UserId() == projeto.ResponsavelId)
            {
                return true;
            }

            if (User.IsInRole(Roles.Fornecedor))
            {
                return await _context.Set<Fornecedor>()
                    .AnyAsync(f =>
                        f.ResponsavelId == this.UserId() && f.Id == projeto.FornecedorId); //Projeto.FornecedorId
            }

            return false;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromRoute] int projetoId)
        {
            if (!await HasAccess(projetoId))
            {
                return Forbid();
            }

            var recursos = Service.Filter(q => q
                .Include(r => r.CategoriaContabil)
                .Where(r => r.ProjetoId == projetoId)
            );
            return Ok(Mapper.Map<List<RecursoMaterialDto>>(recursos));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get([FromRoute] int projetoId, int id)
        {
            if (!await HasAccess(projetoId))
            {
                return Forbid();
            }

            var recurso = Service.Filter(q => q
                .Include(r => r.CategoriaContabil)
                .Where(r => r.ProjetoId == projetoId && r.Id == id)
            ).FirstOrDefault();
            if (recurso == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RecursoMaterialDto>(recurso));
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPut]
        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int projetoId, [FromBody] RecursoMaterialRequest request)
        {
            if (!await HasAccess(projetoId))
            {
                return Forbid();
            }

            var recurso = Mapper.Map<RecursoMaterial>(request);
            recurso.ProjetoId = projetoId;
            if (recurso.Id > 0 && Request.Method == "PUT")
            {
                Service.Put(recurso);
                return Ok();
            }

            if (Request.Method == "POST")
            {
                Service.Post(recurso);
                return Ok();
            }

            return BadRequest();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id, [FromRoute] int projetoId)
        {
            if (!await HasAccess(projetoId))
            {
                return Forbid();
            }

            if (!_context.Set<RegistroFinanceiroInfo>().Any(r => r.RecursoMaterialId == id) &&
                !_context.Set<RecursoMaterial.AlocacaoRm>().Any(a => a.RecursoMaterialId == id))
            {
                Service.Delete(id);
                return Ok();
            }

            return Problem("Não é possível excluir. Recurso usado por registros financeiro e/ou orçamentos",
                statusCode: StatusCodes.Status412PreconditionFailed);
        }
    }
}