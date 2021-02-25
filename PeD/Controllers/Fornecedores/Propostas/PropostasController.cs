using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta Fornecedor")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas")]
    public class PropostasController : ControllerServiceBase<Proposta>
    {
        private IUrlHelper _urlHelper;
        private new PropostaService Service;

        public PropostasController(PropostaService service,
            IMapper mapper, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor) : base(service, mapper)
        {
            Service = service;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HttpGet("")]
        public ActionResult<List<PropostaDto>> GetPropostas()
        {
            var propostas = Service.GetPropostasPorResponsavel(this.UserId());
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        [HttpGet("{id}")]
        public ActionResult<PropostaDto> GetProposta(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            return Mapper.Map<PropostaDto>(proposta);
        }

        [HttpPut("{id}/Rejeitar")]
        public ActionResult RejeitarCaptacao(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Rejeitado;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }

        [HttpPut("{id}/Participar")]
        public ActionResult ParticiparCaptacao(int id)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            if (proposta.Participacao == StatusParticipacao.Pendente)
            {
                proposta.Participacao = StatusParticipacao.Aceito;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }

        [HttpPut("{id}/Duracao")]
        public ActionResult PropostaDuracao(int id, [FromServices] IService<Etapa> etapaService, [FromBody] short meses)
        {
            var proposta = Service.GetPropostaPorResponsavel(id, this.UserId());
            if (proposta.Participacao == StatusParticipacao.Aceito)
            {
                var max = etapaService.Filter(q => q.Where(e => e.PropostaId == proposta.Id))
                    .Select(e => e.Meses.Max())
                    .Max();
                if (meses < max)
                    return Problem("Há etapas em meses superiores à duração desejada",
                        title: "Alteração não permitida", statusCode: StatusCodes.Status428PreconditionRequired);
                proposta.Duracao = meses;
                Service.Put(proposta);
                return Ok();
            }

            return StatusCode(428);
        }
    }
}