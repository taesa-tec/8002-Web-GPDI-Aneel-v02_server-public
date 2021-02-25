using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
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
    public class EtapasController : ControllerServiceBase<Etapa>
    {
        private PropostaService _propostaService;

        public EtapasController(IService<Etapa> service, IMapper mapper, PropostaService propostaService) : base(
            service, mapper)
        {
            _propostaService = propostaService;
        }

        [HttpGet("")]
        public IActionResult Get([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var etapas = Service.Filter(q => q
                .Include(p => p.Produto)
                .OrderBy(e => e.Id)
                .Where(p => p.PropostaId == proposta.Id));
            var etapasDtos = Mapper.Map<List<EtapaDto>>(etapas).Select((dto, i) =>
            {
                dto.Ordem = i + 1;
                return dto;
            });

            return Ok(etapasDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var etapa = Service.Filter(q => q
                .Include(p => p.Produto)
                .Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (etapa == null)
                return NotFound();

            return Ok(Mapper.Map<EtapaDto>(etapa));
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] EtapaRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var etapa = Mapper.Map<Etapa>(request);
            etapa.PropostaId = proposta.Id;
            Service.Post(etapa);
            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromRoute] int captacaoId,
            [FromBody] EtapaRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var etapaPrev = Service
                .Filter(q => q.AsNoTracking().Where(p => p.PropostaId == proposta.Id && p.Id == request.Id))
                .FirstOrDefault();
            if (etapaPrev == null)
                return NotFound();

            var etapa = Mapper.Map<Etapa>(request);
            etapa.PropostaId = proposta.Id;

            Service.Put(etapa);

            return Ok(Mapper.Map<EtapaDto>(etapa));
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] int captacaoId, [FromQuery] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var etapa = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (etapa == null)
                return NotFound();
            Service.Delete(etapa.Id);

            return Ok();
        }
    }
}