using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models.Projetos;
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

        public RecursosMateriaisController(IService<RecursoMaterial> service, IMapper mapper) : base(service, mapper)
        {
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
    }
}