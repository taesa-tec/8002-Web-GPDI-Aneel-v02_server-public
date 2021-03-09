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
using TaesaCore.Models;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class PropostaNodeBaseController<T, TRequest, TResponse> : ControllerServiceBase<T>
        where T : PropostaNode where TRequest : BaseEntity
    {
        protected PropostaService PropostaService;

        public PropostaNodeBaseController(IService<T> service, IMapper mapper, PropostaService propostaService) :
            base(service, mapper)
        {
            PropostaService = propostaService;
        }

        private Proposta _proposta;

        protected Proposta Proposta
        {
            get
            {
                if (_proposta == null && Request.RouteValues.ContainsKey("captacaoId") &&
                    int.TryParse(Request.RouteValues["captacaoId"].ToString(), out int captacaoId))
                {
                    _proposta = PropostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
                }

                return _proposta;
            }
        }

        protected virtual IQueryable<T> Includes(IQueryable<T> queryable)
        {
            return queryable;
        }

        [HttpGet("")]
        public virtual IActionResult Get()
        {
            var nodes = Service.Filter(q => Includes(q)
                .OrderBy(e => e.Id)
                .Where(p => p.PropostaId == Proposta.Id));
            var riscosDtos = Mapper.Map<List<TResponse>>(nodes);
            return Ok(riscosDtos);
        }

        [HttpGet("{id}")]
        public virtual IActionResult Get([FromRoute] int id)
        {
            var node = Service.Filter(q => Includes(q)
                .Where(p => p.PropostaId == Proposta.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();

            return Ok(Mapper.Map<TResponse>(node));
        }

        [HttpPost]
        public virtual ActionResult Post([FromBody] TRequest request)
        {
            var risco = Mapper.Map<T>(request);
            risco.PropostaId = Proposta.Id;
            Service.Post(risco);
            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
            return Ok();
        }

        [HttpPut]
        public virtual IActionResult Put([FromBody] TRequest request)
        {
            var nodeInitial = Service
                .Filter(q => q.AsNoTracking().Where(p => p.PropostaId == Proposta.Id && p.Id == request.Id))
                .FirstOrDefault();

            if (nodeInitial == null)
                return NotFound();

            var node = Mapper.Map<T>(request);
            node.PropostaId = Proposta.Id;

            Service.Put(node);
            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);

            return Ok(Mapper.Map<TResponse>(node));
        }

        [HttpDelete]
        public virtual IActionResult Delete([FromQuery] int id)
        {
            var node = Service.Filter(q => q.Where(p => p.PropostaId == Proposta.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();
            Service.Delete(node.Id);
            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);

            return Ok();
        }
    }
}