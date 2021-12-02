using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.Models.Projetos;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using TaesaCore.Models;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [ApiController]
    [Authorize("Bearer")]
    [Authorize(Roles = "User, Administrador")]
    [Route("api/Projetos/{projetoId:int}/[controller]")]
    public class ProjetoNodeBaseController<T> : ControllerServiceBase<T> where T : BaseEntity
    {
        protected ProjetoService ProjetoService;
        public readonly IAuthorizationService AuthorizationService;

        public ProjetoNodeBaseController(IService<T> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper)
        {
            AuthorizationService = authorizationService;
            ProjetoService = projetoService;
        }

        private Projeto _projeto;

        protected Projeto Projeto
        {
            get
            {
                if (_projeto == null && Request.RouteValues.ContainsKey("projetoId") &&
                    int.TryParse(Request.RouteValues["projetoId"].ToString(), out var id))
                {
                    _projeto = ProjetoService.Get(id);
                }

                return _projeto;
            }
        }

#pragma warning disable 1998
        protected async Task<bool> HasAccess(bool edit = false)
#pragma warning restore 1998
        {
            //@todo Verificar Acesso
            return true;
        }
    }

    [SwaggerTag("Projeto")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Projetos/{projetoId:int}/[controller]")]
    public class ProjetoNodeBaseController<T, TRequest, TResponse> : ProjetoNodeBaseController<T>
        where T : ProjetoNode where TRequest : BaseEntity
    {
        public ProjetoNodeBaseController(IService<T> service, IMapper mapper,
            IAuthorizationService authorizationService, ProjetoService projetoService) : base(service, mapper,
            authorizationService, projetoService)
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
                .Where(p => p.ProjetoId == Projeto.Id));
            var riscosDtos = Mapper.Map<List<TResponse>>(nodes);
            return Ok(riscosDtos);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!await HasAccess())
                return Forbid();
            var node = Service.Filter(q => Includes(q)
                .Where(p => p.ProjetoId == Projeto.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();

            return Ok(Mapper.Map<TResponse>(node));
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TRequest request)
        {
            if (!await HasAccess(true))
                return Forbid();

            var node = Mapper.Map<T>(request);
            node.ProjetoId = Projeto.Id;
            Service.Post(node);
            return Ok(Mapper.Map<TResponse>(node));
        }

        protected virtual void BeforePut(T actual, T @new)
        {
        }

        [HttpPut]
        public virtual async Task<IActionResult> Put([FromBody] TRequest request)
        {
            if (!await HasAccess(true))
                return Forbid();
            var nodeInitial = Service
                .Filter(q => q.AsNoTracking().Where(p => p.ProjetoId == Projeto.Id && p.Id == request.Id))
                .FirstOrDefault();

            if (nodeInitial == null)
                return NotFound();


            var node = Mapper.Map<T>(request);
            node.ProjetoId = Projeto.Id;

            BeforePut(nodeInitial, node);
            Service.Put(node);

            return Ok(Mapper.Map<TResponse>(node));
        }

        [HttpDelete("{id:int}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (!await HasAccess(true))
                return Forbid();
            var node = Service.Filter(q => q.Where(p => p.ProjetoId == Projeto.Id && p.Id == id)).FirstOrDefault();
            if (node == null)
                return NotFound();
            Service.Delete(node.Id);
            return Ok();
        }
    }
}