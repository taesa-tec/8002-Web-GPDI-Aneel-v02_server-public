using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models.Projetos;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos/{id:int}/RegistroFinanceiro")]
    [ApiController]
    [Authorize("Bearer")]
    public class RegistroFinanceiroController : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;

        public RegistroFinanceiroController(ProjetoService service, IMapper mapper) : base(service, mapper)
        {
            Service = service;
        }

        [HttpGet("Criar")]
        public ActionResult GetCriar([FromRoute] int id)
        {
            var rhs = Service.NodeList<RecursoHumano>(id);
            var etapas = Service.NodeList<Etapa>(id);
            var projeto = Service.Get(id);
            var mesesN = etapas.SelectMany(etapa => etapa.Meses).Distinct();
            var meses = mesesN.Select(m => projeto.DataInicioProjeto.AddMonths(m-1));
            return Ok(new
            {
                rhs, etapas, meses
            });
        }
    }
}