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
    public class ProdutosController : PropostaNodeBaseController<Produto, PropostaProdutoRequest, PropostaProdutoDto>
    {
        private GestorDbContext _context;

        public ProdutosController(IService<Produto> service, IMapper mapper, IAuthorizationService authorizationService,
            PropostaService propostaService, GestorDbContext context) : base(service, mapper, authorizationService,
            propostaService)
        {
            _context = context;
        }

        protected override IQueryable<Produto> Includes(IQueryable<Produto> queryable)
        {
            return queryable
                .Include(p => p.FaseCadeia)
                .Include(p => p.TipoDetalhado)
                .Include(p => p.ProdutoTipo);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPost]
        public override async Task<IActionResult> Post(PropostaProdutoRequest request)
        {
            if (!await HasAccess())
                return Forbid();
            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = Proposta.Id;
            produto.Created = DateTime.Now;
            if (produto.Classificacao != ProdutoClassificacao.Final || !_context.Set<Produto>()
                .Any(p => p.Classificacao == ProdutoClassificacao.Final && p.PropostaId == Proposta.Id))
            {
                Service.Post(produto);

                PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
                return Ok();
            }

            return Problem("Somente um produto final por proposta", null, StatusCodes.Status409Conflict);
        }

        [Authorize(Roles = Roles.Fornecedor)]
        [HttpPut]
        public override async Task<IActionResult> Put(PropostaProdutoRequest request)
        {
            if (!await HasAccess())
                return Forbid();
            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = Proposta.Id;
            produto.Created = DateTime.Now;
            if (produto.Classificacao != ProdutoClassificacao.Final || !_context.Set<Produto>()
                .Any(p =>
                    p.Classificacao == ProdutoClassificacao.Final &&
                    p.PropostaId == Proposta.Id &&
                    p.Id != request.Id))
            {
                Service.Put(produto);
                PropostaService.UpdatePropostaDataAlteracao(Proposta.Id);
                return Ok();
            }

            return Problem("Somente um produto final por proposta", null, StatusCodes.Status409Conflict);
        }

        public override async Task<IActionResult> Delete(int id)
        {
            if (!await HasAccess())
                return Forbid();
            if (_context.Set<Etapa>().AsQueryable().Any(a => a.ProdutoId == id))
            {
                return Problem("Não é possível apagar um produto associado a uma etapa", null,
                    StatusCodes.Status409Conflict);
            }

            return await base.Delete(id);
        }
    }
}