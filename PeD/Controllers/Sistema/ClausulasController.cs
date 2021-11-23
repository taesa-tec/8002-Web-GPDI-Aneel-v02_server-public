using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models;
using PeD.Data;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [SwaggerTag("Contrato Base")]
    [Route("api/Sistema/Clausulas")]
    [ApiController]
    [Authorize("Bearer")]
    public class ClausulasController : ControllerServiceBase<Clausula>
    {
        public ClausulasController(IService<Clausula> service, IMapper mapper) : base(service, mapper)
        {
        }

        [HttpGet]
        public IList<Clausula> Index()
        {
            return Service.Get().OrderBy(c => c.Ordem).ToList();
        }

        [Authorize(Policy = Policies.IsAdmin)]
        [HttpPost]
        public ActionResult Save(List<ClausulaDto> clausulas, [FromServices] GestorDbContext context)
        {
            int o = 0;
            var ids = clausulas.Where(c => c.Id > 0).Select(c => c.Id);
            var excluidas = context.Set<Clausula>().Where(c => !ids.Contains(c.Id)).ToList();
            context.RemoveRange(excluidas);
            context.SaveChanges();
            clausulas.ForEach(clausula =>
            {
                var c = Mapper.Map<Clausula>(clausula);
                c.Ordem = o++;
                if (clausula.Id > 0)
                {
                    Service.Put(c);
                }
                else
                {
                    Service.Post(c);
                }
            });
            return Ok();
        }
    }
}