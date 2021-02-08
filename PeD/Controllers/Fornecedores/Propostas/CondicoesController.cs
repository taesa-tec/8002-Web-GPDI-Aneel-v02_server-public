using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    //[Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class CondicoesController : Controller
    {
        private IService<Clausula> Service;

        public CondicoesController(IService<Clausula> service)
        {
            Service = service;
        }

        [SwaggerOperation("Lista das clausulas")]
        [HttpGet("/api/Fornecedor/Clausulas")]
        public ActionResult<List<Clausula>> Index()
        {
            var clausulas = Service.Get();
            return Ok(clausulas);
        }

        [SwaggerOperation("Aceitação dos termos das clausulas")]
        [HttpPost("{id}")]
        public ActionResult AceitarClausula(int id, bool accepted)
        {
            return Ok();
        }
    }
}