using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProdutosController : ControllerServiceBase<Produto>
    {
        private PropostaService _propostaService;
        private GestorDbContext _context;

        public ProdutosController(IService<Produto> service, IMapper mapper, PropostaService propostaService,
            GestorDbContext context) : base(
            service, mapper)
        {
            _propostaService = propostaService;
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Get([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produtos = Service.Filter(q => q
                .Include(p => p.FaseCadeia)
                .Include(p => p.TipoDetalhado)
                .Include(p => p.ProdutoTipo)
                .Where(p => p.PropostaId == proposta.Id));

            return Ok(Mapper.Map<List<PropostaProdutoDto>>(produtos));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Service.Filter(q => q
                .Include(p => p.FaseCadeia)
                .Include(p => p.TipoDetalhado)
                .Include(p => p.ProdutoTipo)
                .Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (produto == null)
                return NotFound();

            return Ok(Mapper.Map<PropostaProdutoDto>(produto));
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] PropostaProdutoRequest request,
            [FromServices] GestorDbContext context)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = proposta.Id;
            produto.Created = DateTime.Now;
            if (produto.Classificacao != ProdutoClassificacao.Final || !context.Set<Produto>()
                .Any(p => p.Classificacao == ProdutoClassificacao.Final && p.PropostaId == proposta.Id))
            {
                Service.Post(produto);

                _propostaService.UpdatePropostaDataAlteracao(proposta.Id);
                return Ok();
            }

            return Problem("Somente um produto final por proposta", null, StatusCodes.Status409Conflict);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public IActionResult Put([FromRoute] int captacaoId,
            [FromBody] PropostaProdutoRequest request,
            [FromServices] GestorDbContext context)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produtoPrev = Service
                .Filter(q => q.AsNoTracking().Where(p => p.PropostaId == proposta.Id && p.Id == request.Id))
                .FirstOrDefault();
            if (produtoPrev == null)
                return NotFound();

            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = proposta.Id;
            produto.Created = produtoPrev.Created;

            if (produto.Classificacao != ProdutoClassificacao.Final || !context.Set<Produto>()
                .Any(p => p.Classificacao == ProdutoClassificacao.Final && p.PropostaId == proposta.Id &&
                          p.Id != produto.Id))
            {
                Service.Put(produto);
                _propostaService.UpdatePropostaDataAlteracao(proposta.Id);
                return Ok(Mapper.Map<PropostaProdutoDto>(produto));
            }

            return Problem("Somente um produto final por proposta", null, StatusCodes.Status409Conflict);
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] int captacaoId, [FromQuery] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (produto == null)
                return NotFound();

            if (_context.Set<Etapa>().AsQueryable().Any(a => a.ProdutoId == id))
            {
                return Problem("Não é possível apagar um produto associado a uma etapa", null,
                    StatusCodes.Status409Conflict);
            }

            Service.Delete(produto.Id);
            _propostaService.UpdatePropostaDataAlteracao(proposta.Id);

            return Ok();
        }
    }
}