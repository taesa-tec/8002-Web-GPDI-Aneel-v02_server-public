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
using Empresa = PeD.Core.Models.Propostas.Empresa;

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{propostaId:guid}/[controller]")]
    public class EmpresasController : PropostaNodeBaseController<Empresa, EmpresaRequest, EmpresaDto>
    {
        new private EmpresaService Service;
        private GestorDbContext _context;

        public EmpresasController(EmpresaService service, IMapper mapper,
            IAuthorizationService authorizationService, PropostaService propostaService, GestorDbContext context) :
            base(service, mapper, authorizationService, propostaService)
        {
            Service = service;
            _context = context;
        }


        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] EmpresaRequest request)
        {
            if (!await HasAccess(true))
                return Forbid();

            var empresa = new Empresa()
            {
                PropostaId = Proposta.Id,
                RazaoSocial = request.RazaoSocial,
                UF = request.UF,
                CNPJ = request.CNPJ,
                Funcao = request.Funcao,
                Codigo = request.Codigo
            };
            Service.Save(empresa);
            return Ok();
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public override async Task<IActionResult> Put([FromBody] EmpresaRequest request)
        {
            if (!await HasAccess(true))
                return Forbid();

            var empresa = Service.Get(request.Id);
            if (empresa.PropostaId == Proposta.Id)
            {
                empresa.RazaoSocial = request.RazaoSocial;
                empresa.UF = request.UF;
                empresa.CNPJ = request.CNPJ;
                empresa.Funcao = request.Funcao;
                empresa.Codigo = request.Codigo;
                Service.Save(empresa);
                PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
                return Ok();
            }

            return Forbid();
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpDelete("{id:int}")]
        public override async Task<IActionResult> Delete(int id)
        {
            if (!await HasAccess(true))
                return Forbid();

            var empresa = Service.Get(id);
            if (empresa == null)
            {
                return NotFound();
            }

            if (empresa.PropostaId == Proposta.Id)
            {
                if (_context.Set<AlocacaoRm>().AsQueryable().Any(a =>
                        a.EmpresaRecebedoraId == id || a.EmpresaFinanciadoraId == id) ||
                    _context.Set<AlocacaoRh>().AsQueryable().Any(a => a.EmpresaFinanciadoraId == id))
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