using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Fornecedor.Controllers.Propostas
{
    [SwaggerTag("Proposta - Detalhes")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{propostaId:int}/[controller]")]
    public class RecursosMateriaisController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}