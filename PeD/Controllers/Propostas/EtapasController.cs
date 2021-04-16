using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class EtapasController : PropostaNodeBaseController<Etapa, EtapaRequest, EtapaDto>
    {
        private GestorDbContext _context;

        public EtapasController(IService<Etapa> service, IMapper mapper, IAuthorizationService authorizationService,
            PropostaService propostaService, GestorDbContext context) : base(service, mapper, authorizationService,
            propostaService)
        {
            _context = context;
        }

        protected override IQueryable<Etapa> Includes(IQueryable<Etapa> queryable)
        {
            return queryable.Include(p => p.Produto);
        }

        protected void UpdateOrder()
        {
            var etapas = Service.Filter(q => q
                .Include(p => p.Produto)
                .OrderBy(e => e.Id)
                .Where(p => p.PropostaId == Proposta.Id)).ToList();
            short o = 1;
            etapas.ForEach(etapa => etapa.Ordem = o++);
            Service.Put(etapas);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public override async Task<IActionResult> Post(EtapaRequest request)
        {
            var result = await base.Post(request);
            UpdateOrder();
            return result;
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public override async Task<IActionResult> Put(EtapaRequest request)
        {
            var result = await base.Put(request);
            UpdateOrder();
            return result;
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpDelete]
        public override async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await base.Delete(id);
            UpdateOrder();
            return result;
        }
    }
}