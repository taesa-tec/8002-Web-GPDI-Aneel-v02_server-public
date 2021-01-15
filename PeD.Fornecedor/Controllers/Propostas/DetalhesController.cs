using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.FornecedoresDtos;
using PeD.Core.Models;
using PeD.Fornecedor.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Fornecedor.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{propostaId:int}/[controller]")]
    public class DetalhesController : Controller
    {
        private IMapper Mapper;
        private PropostaService Service;

        public DetalhesController(PropostaService service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index([FromRoute] int propostaId)
        {
            var proposta = Service.GetProposta(propostaId);
            return Ok(Mapper.Map<PropostaDto>(proposta));
        }
    }
}