using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using TaesaCore.Models;

namespace PeD.Controllers.Propostas
{
    public class PropostaNodeBaseController<T> : ControllerServiceBase<T> where T : BaseEntity
    {
        protected PropostaService PropostaService;
        public readonly IAuthorizationService AuthorizationService;

        public PropostaNodeBaseController(IService<T> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper)
        {
            AuthorizationService = authorizationService;
            PropostaService = propostaService;
        }

        private Proposta _proposta;

        protected Proposta Proposta
        {
            get
            {
                if (_proposta == null && Request.RouteValues.ContainsKey("propostaId") &&
                    Guid.TryParse(Request.RouteValues["propostaId"].ToString(), out Guid guid))
                {
                    _proposta = PropostaService.GetProposta(guid);
                }

                return _proposta;
            }
        }

        protected async Task<bool> HasAccess()
        {
            var authorizationResult = await this.Authorize(Proposta);
            return authorizationResult.Succeeded;
        }
    }

    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class PropostaNodeBaseController<T, TRequest, TResponse> : PropostaNodeBaseController<T>
        where T : PropostaNode where TRequest : BaseEntity
    {
        public PropostaNodeBaseController(IService<T> service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService) : base(service, mapper,
            authorizationService, propostaService)
        {
        }


        protected virtual IQueryable<T> Includes(IQueryable<T> queryable)
        {
            return queryable;
        }

        [HttpGet("")]
        public virtual async Task<IActionResult> Get()
        {
            if (!await HasAccess())
                return Forbid();

            var nodes = Service.Filter(q => Includes(q)
                .OrderBy(e => e.Id)
                .Where(p => p.PropostaId == Proposta.Id));
            var riscosDtos = Mapper.Map<List<TResponse>>(nodes);
            return Ok(riscosDtos);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!await HasAccess())
                return Forbid();
            var node = Service.Filter(q => Includes(q)
                .Where(p => p.PropostaId == Proposta.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();

            return Ok(Mapper.Map<TResponse>(node));
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TRequest request)
        {
            if (!await HasAccess())
                return Forbid();

            var risco = Mapper.Map<T>(request);
            risco.PropostaId = Proposta.Id;
            Service.Post(risco);
            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
            return Ok();
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TRequest request)
        {
            if (!await HasAccess())
                return Forbid();
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

        [HttpDelete("{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await HasAccess())
                return Forbid();
            var node = Service.Filter(q => q.Where(p => p.PropostaId == Proposta.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();
            Service.Delete(node.Id);
            PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);

            return Ok();
        }
    }
}