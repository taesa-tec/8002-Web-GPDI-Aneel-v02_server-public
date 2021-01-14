using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Fornecedor.Propostas
{
    [SwaggerTag("Proposta - Detalhes")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{propostaId:int}/[controller]")]
    public class RecursosHumanoController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}