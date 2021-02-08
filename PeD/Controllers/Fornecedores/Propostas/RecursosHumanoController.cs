using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class RecursosHumanoController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}