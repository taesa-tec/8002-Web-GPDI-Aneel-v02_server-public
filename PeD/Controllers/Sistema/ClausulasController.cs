using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Sistema;
using PeD.Core.Models.Captacao;
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

        [HttpPost]
        public ActionResult Save(List<ClausulaDto> clausulas)
        {
            int o = 0;
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