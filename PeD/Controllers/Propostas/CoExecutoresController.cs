using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    //PropostaNodeBaseController<Risco, RiscoRequest, PropostaRiscoDto>
    public class CoExecutoresController : PropostaNodeBaseController<CoExecutor, CoExecutorRequest, CoExecutorDto>
    {
        new private CoExecutorService Service;
        private GestorDbContext _context;

        public CoExecutoresController(CoExecutorService service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper, authorizationService, propostaService)
        {
            Service = service;
            _context = context;
        }


        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] CoExecutorRequest request)
        {
            if (!await HasAccess())
                return Forbid();

            var coExecutor = new CoExecutor()
            {
                PropostaId = Proposta.Id,
                RazaoSocial = request.RazaoSocial,
                UF = request.UF,
                CNPJ = request.CNPJ,
                Funcao = request.Funcao
            };
            Service.Save(coExecutor);
            return Ok();
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] CoExecutorRequest request)
        {
            if (!await HasAccess())
                return Forbid();

            var coExecutor = Service.Get(request.Id);
            if (coExecutor.PropostaId == Proposta.Id)
            {
                coExecutor.Id = request.Id;
                coExecutor.PropostaId = Proposta.Id;
                coExecutor.RazaoSocial = request.RazaoSocial;
                coExecutor.UF = request.UF;
                coExecutor.CNPJ = request.CNPJ;
                coExecutor.Funcao = request.Funcao;
                Service.Save(coExecutor);
                PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
                return Ok();
            }

            return Forbid();
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpDelete]
        public override async Task<IActionResult> Delete([FromQuery] int id)
        {
            if (!await HasAccess())
                return Forbid();

            var coExecutor = Service.Get(id);
            if (coExecutor == null)
            {
                return NotFound();
            }

            if (coExecutor.PropostaId == Proposta.Id)
            {
                if (_context.Set<RecursoMaterial.AlocacaoRm>().AsQueryable().Any(a =>
                        a.CoExecutorRecebedorId == id || a.CoExecutorFinanciadorId == id) ||
                    _context.Set<RecursoHumano.AlocacaoRh>().AsQueryable().Any(a => a.CoExecutorFinanciadorId == id))
                {
                    return Problem("Não é possível apagar uma entidade relacionada com alocações de recursos", null,
                        StatusCodes.Status409Conflict);
                }

                Service.Delete(id);
                PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
                return Ok();
            }

            return Forbid();
        }
    }
}