using APIGestor.Models.Captacao;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace APIGestor.Controllers.Sistema
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
    }
}