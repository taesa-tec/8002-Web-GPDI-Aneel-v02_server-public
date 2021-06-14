using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Projetos;
using PeD.Core.Requests.Projetos;
using PeD.Data;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProjetoController : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;
        protected GestorDbContext Context;

        public ProjetoController(ProjetoService service, IMapper mapper, GestorDbContext context) : base(service,
            mapper)
        {
            Service = service;
            Context = context;
        }

        [HttpGet("EmExecucao")]
        public ActionResult GetEmExecucao()
        {
            var projetos = Service.Filter(q => q.Where(p => p.Status == Status.Execucao));
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("Finalizados")]
        public ActionResult GetFinalizados()
        {
            var projetos = Service.Filter(q => q.Where(p => p.Status == Status.Finalizado));
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var projeto = Service.Filter(q =>
                q.Include(p => p.Proponente)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return Ok(Mapper.Map<ProjetoDto>(projeto));
        }

        [HttpGet("{id:int}/Empresas")]
        public ActionResult Empresas(int id, [FromServices] IService<Empresa> empresasService)
        {
            var projeto = Service.Get(id);
            var coExecutores = Context.Set<CoExecutor>().AsQueryable().Where(c => c.ProjetoId == id).ToList();
            var empresas = empresasService.Filter(q =>
                q.Where(e => e.Categoria == Empresa.CategoriaEmpresa.Taesa).OrderBy(e => e.Id));
            var fornecedor = Context.Set<Fornecedor>().AsQueryable().FirstOrDefault(c => c.Id == projeto.FornecedorId);
            empresas.Add(fornecedor);

            var response = new
            {
                empresas,
                coExecutores = Mapper.Map<List<CoExecutorDto>>(coExecutores)
            };
            return Ok(response);
        }

        [HttpGet("{id:int}/Produtos")]
        public ActionResult Produtos(int id)
        {
            var produtos = Context.Set<Produto>()
                .Where(p => p.ProjetoId == id)
                .ToList();

            return Ok(Mapper.Map<List<ProjetoProdutoDto>>(produtos));
        }

        [HttpPost("{id:int}/Prorrogacao")]
        public ActionResult Prorrogacao(int id, ProrrogacaoRequest request)
        {
            var projeto = Service.Get(id);
            if (request.Data < projeto.DataFinalProjeto)
            {
                return BadRequest("Data inválida!");
            }

            var etapaMeses = new List<int>();
            var dateInicio = projeto.DataInicioProjeto;
            var dateFinal = projeto.DataFinalProjeto;
            var newDateFinal = request.Data;
            var mesesTotal = 1 + (dateFinal.Year - dateInicio.Year) * 12 + dateFinal.Month - dateInicio.Month;
            var etapaMesesTotal = (newDateFinal.Year - dateFinal.Year) * 12 + newDateFinal.Month - dateFinal.Month;
            for (int i = 1; i <= etapaMesesTotal; i++)
            {
                etapaMeses.Add(mesesTotal + i);
            }

            projeto.DataFinalProjeto = request.Data;

            var ordem = Context.Set<Etapa>().Where(e => e.ProjetoId == id).Max(e => e.Ordem) + 1;

            var etapa = new Etapa()
            {
                ProjetoId = id,
                ProdutoId = request.ProdutoId,
                DescricaoAtividades = request.Descricao,
                Meses = etapaMeses,
                Ordem = (short) ordem
            };
            Context.Add(etapa);
            Context.Update(projeto);
            Context.SaveChanges();
            return Ok();
        }
    }
}