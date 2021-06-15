using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
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

        [HttpGet]
        public ActionResult Get([FromRoute] int projetoId)
        {
            var recursos = Service.Filter(q => q
                .Include(r => r.CategoriaContabil)
                .Where(r => r.ProjetoId == projetoId)
            );
            return Ok(Mapper.Map<List<RecursoMaterialDto>>(recursos));
        }

        [HttpGet("{id:int}")]
        public ActionResult Get([FromRoute] int projetoId, int id)
        {
            var recurso = Service.Filter(q => q
                .Include(r => r.CategoriaContabil)
                .Where(q => q.ProjetoId == projetoId && q.Id == id)
            ).FirstOrDefault();
            if (recurso == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<RecursoMaterialDto>(recurso));
        }

        [HttpPut]
        [HttpPost]
        public ActionResult Post([FromRoute] int projetoId, [FromBody] RecursoMaterialRequest request)
        {
            var recurso = Mapper.Map<RecursoMaterial>(request);
            recurso.ProjetoId = projetoId;
            if (recurso.Id > 0 && Request.Method == "PUT")
            {
                Service.Put(recurso);
                return Ok();
            }
            else if (Request.Method == "POST")
            {
                Service.Post(recurso);
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            if (!_context.Set<RegistroFinanceiroInfo>().Any(r => r.RecursoMaterialId == id))
            {
                Service.Delete(id);
                return Ok();
            }

            return Problem("Não é possível excluir. Recurso atrelado a registros financeiros",
                statusCode: StatusCodes.Status412PreconditionFailed);
        }
    }
}