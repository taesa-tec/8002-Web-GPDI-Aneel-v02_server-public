using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Data;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class ContratosController : Controller
    {
        private PropostaService propostaService;
        private CaptacaoService captacaoService;
        private IService<PropostaContrato> Service;
        private GestorDbContext _context;
        private IMapper mapper;

        public ContratosController(PropostaService propostaService, IMapper mapper, CaptacaoService captacaoService,
            IService<PropostaContrato> service, GestorDbContext context)
        {
            this.propostaService = propostaService;
            this.mapper = mapper;
            this.captacaoService = captacaoService;
            Service = service;
            _context = context;
        }

        [HttpGet("")]
        public ActionResult<List<ContratoListItemDto>> Index([FromRoute] int captacaoId)
        {
            var contratosPropostas = propostaService.GetContratos(captacaoId, this.UserId());
            var contratos = captacaoService.GetContratos(captacaoId);
            var ausentes = contratos.Where(c => !contratosPropostas.Any(cp => cp.ParentId == c.Id))
                .Select(c => new PropostaContrato()
                {
                    ParentId = c.Id,
                    Parent = c
                });
            //contratosPropostas = ;
            return mapper.Map<List<ContratoListItemDto>>(contratosPropostas.Concat(ausentes));
        }

        [HttpGet("{contratoId}")]
        public ActionResult<ContratoDto> Get([FromRoute] int captacaoId, [FromRoute] int contratoId)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var contratoProposta = propostaService.GetContrato(contratoId, proposta.Id);
            if (contratoProposta != null)
                return mapper.Map<ContratoDto>(contratoProposta);
            var contrato = captacaoService.GetContrato(contratoId);

            return mapper.Map<ContratoDto>(new PropostaContrato()
            {
                ParentId = contrato.Id,
                Parent = contrato
            });
        }

        [HttpPost("{contratoId}")]
        public ActionResult Post([FromRoute] int captacaoId, [FromRoute] int contratoId,
            [FromBody] ContratoRequest request)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var contrato = captacaoService.GetContrato(contratoId);

            if (proposta == null || contrato == null)
            {
                return NotFound();
            }

            var contratoProposta = propostaService.GetContrato(contratoId, proposta.Id);
            if (contratoProposta != null)
            {
                contratoProposta.Finalizado = !request.Draft;
                contratoProposta.Conteudo = request.Conteudo;
                Service.Put(contratoProposta);
            }
            else
            {
                contratoProposta = new PropostaContrato()
                {
                    Conteudo = request.Conteudo,
                    Finalizado = !request.Draft,
                    ParentId = contrato.Id,
                    PropostaId = proposta.Id
                };
                Service.Post(contratoProposta);
            }

            var revisao = contratoProposta.ToRevisao();
            revisao.UserId = this.UserId();
            _context.Add(revisao);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("{contratoId}/Revisoes")]
        public ActionResult<List<BaseEntityDto>> GetRevisoes([FromRoute] int captacaoId,
            [FromRoute] int contratoId)
        {
            var proposta = propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var revisoes = propostaService.GetContratoRevisoes(contratoId, proposta.Id);
            return mapper.Map<List<BaseEntityDto>>(revisoes);
        }
    }
}