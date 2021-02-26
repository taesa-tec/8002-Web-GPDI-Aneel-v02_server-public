using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class RiscosController : PropostaNodeBaseController<Risco, RiscoRequest, PropostaRiscoDto>
    {
        public RiscosController(IService<Risco> service, IMapper mapper, PropostaService propostaService) : base(
            service, mapper, propostaService)
        {
        }


        /*
        [HttpGet("")]
        public IActionResult Get([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var riscos = Service.Filter(q => q
                .OrderBy(e => e.Id)
                .Where(p => p.PropostaId == proposta.Id));
            var riscosDtos = Mapper.Map<List<PropostaRiscoDto>>(riscos);
            return Ok(riscosDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var risco = Service.Filter(q => q
                .Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (risco == null)
                return NotFound();

            return Ok(Mapper.Map<PropostaRiscoDto>(risco));
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] RiscoRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var risco = Mapper.Map<Risco>(request);
            risco.PropostaId = proposta.Id;
            Service.Post(risco);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromRoute] int captacaoId,
            [FromBody] RiscoRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var riscoPrev = Service
                .Filter(q => q.AsNoTracking().Where(p => p.PropostaId == proposta.Id && p.Id == request.Id))
                .FirstOrDefault();
            if (riscoPrev == null)
                return NotFound();

            var risco = Mapper.Map<Risco>(request);
            risco.PropostaId = proposta.Id;

            Service.Put(risco);

            return Ok(Mapper.Map<PropostaRiscoDto>(risco));
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] int captacaoId, [FromQuery] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var risco = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (risco == null)
                return NotFound();
            Service.Delete(risco.Id);

            return Ok();
        }
        */
    }
}