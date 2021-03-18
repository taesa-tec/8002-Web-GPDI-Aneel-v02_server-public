using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DiffPlex.DiffBuilder.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Extensions;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class ContratoController : Controller
    {
        private PropostaService propostaService;
        private CaptacaoService captacaoService;
        private IService<PropostaContrato> Service;
        private GestorDbContext _context;
        private IMapper mapper;

        public ContratoController(PropostaService propostaService, IMapper mapper, CaptacaoService captacaoService,
            IService<PropostaContrato> service, GestorDbContext context)
        {
            this.propostaService = propostaService;
            this.mapper = mapper;
            this.captacaoService = captacaoService;
            Service = service;
            _context = context;
        }

        [HttpGet("")]
        public ActionResult<PropostaContratoDto> Get([FromRoute] int captacaoId)
        {
            var contrato = propostaService.GetContrato(captacaoId, this.UserId());
            return Ok(mapper.Map<PropostaContratoDto>(contrato));
        }

        [HttpPost("")]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] ContratoRequest request)
        {
            var contratoProposta = propostaService.GetContrato(captacaoId, this.UserId());
            var hash = contratoProposta.Conteudo?.ToMD5() ?? "";
            contratoProposta.Finalizado = !request.Draft;
            contratoProposta.Conteudo = request.Conteudo;
            if (contratoProposta.Id != 0)
            {
                Service.Put(contratoProposta);
            }
            else
            {
                var parent = contratoProposta.Parent;
                contratoProposta.Parent = null;
                Service.Post(contratoProposta);
                contratoProposta.Parent = parent;
            }

            // Só criar revisão se tiver alguma alteração de texto relevante
            if (!hash.Equals(request.Conteudo.ToMD5()))
            {
                var revisao = contratoProposta.ToRevisao();
                revisao.UserId = this.UserId();
                _context.Add(revisao);
                _context.SaveChanges();
            }
            propostaService.UpdatePropostaDataAlteracao(contratoProposta.PropostaId);

            return Ok();
        }

        [HttpGet("Revisoes")]
        public ActionResult<List<ContratoRevisaoListItemDto>> GetRevisoes([FromRoute] int captacaoId)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var revisoes = propostaService.GetContratoRevisoes(proposta.Id);
            return mapper.Map<List<ContratoRevisaoListItemDto>>(revisoes);
        }

        [HttpGet("Revisoes/{id}")]
        public ActionResult<ContratoRevisaoDto> GetRevisao([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var revisao = propostaService.GetContratoRevisao(proposta.Id, id);
            return mapper.Map<ContratoRevisaoDto>(revisao);
        }

        [HttpGet("Revisoes/{id}/Diff")]
        public async Task<ActionResult<string>> GetRevisaoDiff([FromRoute] int captacaoId, [FromRoute] int id,
            [FromServices] IViewRenderService viewRenderService)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var contrato = propostaService.GetContrato(captacaoId, this.UserId());
            var revisao = propostaService.GetContratoRevisao(proposta.Id, id);

            var diff = DiffService.Html(contrato.Conteudo,
                revisao.Conteudo);
            var render = await viewRenderService.RenderToStringAsync("Pdf/Diff", diff);
            return render;
        }
    }
}