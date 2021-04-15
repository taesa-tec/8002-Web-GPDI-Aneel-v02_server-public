using System.Collections.Generic;
using System.Linq;
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

namespace PeD.Controllers.Propostas
{
    [SwaggerTag("Proposta")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/Propostas/{captacaoId:int}/[controller]")]
    public class CoExecutoresController : ControllerBase
    {
        private PropostaService propostaService;
        private CoExecutorService Service;
        private IMapper mapper;
        private GestorDbContext _context;

        public CoExecutoresController(CoExecutorService service, IMapper mapper, PropostaService propostaService, GestorDbContext context)
        {
            Service = service;
            this.mapper = mapper;
            this.propostaService = propostaService;
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<CoExecutorDto>> Get([FromRoute] int captacaoId)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var ces = Service.GetPorProposta(proposta.Id);
            return mapper.Map<List<CoExecutorDto>>(ces);
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] CoExecutorRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());

            var coExecutor = new CoExecutor()
            {
                PropostaId = proposta.Id,
                RazaoSocial = request.RazaoSocial,
                UF = request.UF,
                CNPJ = request.CNPJ,
                Funcao = request.Funcao
            };
            Service.Save(coExecutor);
            return Ok();
        }

        [HttpPut]
        public ActionResult Put([FromRoute] int captacaoId, [FromBody] CoExecutorRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var coExecutor = Service.Get(request.Id);
            if (coExecutor.PropostaId == proposta.Id)
            {
                coExecutor.Id = request.Id;
                coExecutor.PropostaId = proposta.Id;
                coExecutor.RazaoSocial = request.RazaoSocial;
                coExecutor.UF = request.UF;
                coExecutor.CNPJ = request.CNPJ;
                coExecutor.Funcao = request.Funcao;
                Service.Save(coExecutor);
                propostaService.UpdatePropostaDataAlteracao(proposta.Id);
                return Ok();
            }

            return Forbid();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var coExecutor = Service.Get(id);
            if (coExecutor == null)
            {
                return NotFound();
            }

            if (coExecutor.PropostaId == proposta.Id)
            {
                
                if (_context.Set<RecursoMaterial.AlocacaoRm>().AsQueryable().Any(a => a.CoExecutorRecebedorId == id || a.CoExecutorFinanciadorId == id) ||
                    _context.Set<RecursoHumano.AlocacaoRh>().AsQueryable().Any(a => a.CoExecutorFinanciadorId == id))
                {
                    return Problem("Não é possível apagar uma entidade relacionada com alocações de recursos", null, StatusCodes.Status409Conflict);
                }
                Service.Delete(id);
                propostaService.UpdatePropostaDataAlteracao(proposta.Id);
                return Ok();
            }

            return Forbid();
        }
    }
}