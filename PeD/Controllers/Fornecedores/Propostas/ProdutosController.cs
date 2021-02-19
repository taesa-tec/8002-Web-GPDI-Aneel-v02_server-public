using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Propostas;
using PeD.Core.Requests.Proposta;
using PeD.Services.Captacoes;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Fornecedores.Propostas
{
    [SwaggerTag("Proposta ")]
    [ApiController]
    [Authorize("Bearer", Roles = Roles.Fornecedor)]
    [Route("api/Fornecedor/Propostas/{captacaoId:int}/[controller]")]
    public class ProdutosController : ControllerServiceBase<Produto>
    {
        private PropostaService _propostaService;

        public ProdutosController(IService<Produto> service, IMapper mapper, PropostaService propostaService) : base(
            service, mapper)
        {
            _propostaService = propostaService;
        }

        [HttpGet("")]
        public IActionResult Get([FromRoute] int captacaoId)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produtos = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id));

            return Ok(Mapper.Map<List<PropostaProdutoDto>>(produtos));
        }

        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (produto == null)
                return NotFound();

            return Ok(Mapper.Map<PropostaProdutoDto>(produto));
        }

        [HttpPost]
        public ActionResult Post([FromRoute] int captacaoId, [FromBody] PropostaProdutoRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = proposta.Id;
            produto.Created = DateTime.Now;
            Service.Post(produto);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int captacaoId, [FromRoute] int id,
            [FromBody] PropostaProdutoRequest request)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produtoPrev = Service
                .Filter(q => q.AsNoTracking().Where(p => p.PropostaId == proposta.Id && p.Id == id))
                .FirstOrDefault();
            if (produtoPrev == null)
                return NotFound();

            var produto = Mapper.Map<Produto>(request);
            produto.PropostaId = proposta.Id;
            produto.Created = produtoPrev.Created;

            Service.Put(produto);

            return Ok(Mapper.Map<PropostaProdutoDto>(produto));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int captacaoId, [FromRoute] int id)
        {
            var proposta = _propostaService.GetPropostaPorResponsavel(captacaoId, this.UserId());
            var produto = Service.Filter(q => q.Where(p => p.PropostaId == proposta.Id && p.Id == id)).FirstOrDefault();
            if (produto == null)
                return NotFound();
            Service.Delete(produto.Id);

            return Ok(Mapper.Map<PropostaProdutoDto>(produto));
        }
    }
}