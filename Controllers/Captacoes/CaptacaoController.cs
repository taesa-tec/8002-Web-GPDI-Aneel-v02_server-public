using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIGestor.Data;
using APIGestor.Dtos.Captacao;
using APIGestor.Models.Captacao;
using APIGestor.Requests.Captacao;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using TaesaCore.Models;

namespace APIGestor.Controllers.Captacoes
{
    [SwaggerTag("Captacao")]
    [Route("api/Captacoes")]
    [ApiController]
    [Authorize("Bearer")]
    public class CaptacaoController : ControllerServiceBase<Captacao>
    {
        public CaptacaoController(IService<Captacao> service, IMapper mapper) : base(service, mapper)
        {
        }

        [HttpGet("Pendentes")]
        public ActionResult<List<CaptacaoPendenteDto>> GetPendentes()
        {
            //Service.Paged()
            var captacoes = Service.Get(q =>
                q.Include(c => c.Criador).Where(c => c.Status == Captacao.CaptacaoStatus.Pendente));
            var mapped = Mapper.Map<List<CaptacaoPendenteDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Elaboracao")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetEmElaboracao()
        {
            //Service.Paged()
            var captacoes = Service.Get(q => q.Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao));
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Canceladas")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetCanceladas()
        {
            //Service.Paged()
            var captacoes =
                Service.Get(q => q.Where(c => c.Status == Captacao.CaptacaoStatus.Cancelada)); // ou com zero propostas
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Abertas")]
        public ActionResult<List<CaptacaoDto>> GetAbertas()
        {
            //Service.Paged()
            var captacoes =
                Service.Get(q =>
                    q.Where(c =>
                        c.Status == Captacao.CaptacaoStatus.Fornecedor &&
                        c.Termino > DateTime.Today));
            var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Encerradas")]
        public ActionResult<List<CaptacaoDto>> GetEncerradas()
        {
            //Service.Paged()
            var captacoes =
                Service.Get(q =>
                    q.Where(c =>
                        c.Status == Captacao.CaptacaoStatus.Fornecedor &&
                        c.Termino <= DateTime.Today));
            var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(mapped);
        }


        [HttpPost("NovaCaptacao")]
        public async Task NovaCaptacao(NovaCaptacaoRequest request, [FromServices] GestorDbContext context)
        {
            var captacao = Service.Get(request.Id);
            captacao.Observacoes = request.Observacoes;
            captacao.EnvioCaptacao = DateTime.Now;
            Service.Put(captacao);
            if (request.FornecedorsSugeridos.Count > 0)
            {
                var fornecedoresSugeridos = request.FornecedorsSugeridos.Select(fs => new CaptacaoSugestaoFornecedor()
                {
                    CaptacaoId = request.Id,
                    FornecedorId = fs
                });

                var dbset = context.Set<CaptacaoSugestaoFornecedor>();
                // dbset.RemoveRange(dbset.Where(csf => csf.CaptacaoId == request.Id));
                await dbset.AddRangeAsync(fornecedoresSugeridos);
            }
        }

        [HttpPut("Cancelar")]
        public ActionResult Cancelar(BaseEntity request)
        {
            var captacao = Service.Get(request.Id);

            captacao.Status = Captacao.CaptacaoStatus.Cancelada;
            captacao.Cancelamento = DateTime.Now;
            Service.Put(captacao);
            return Ok();
        }

        public class CaptacaoPrazoRequest : BaseEntity
        {
            public DateTime Termino { get; set; }
        }

        [HttpPut("AlterarPrazo")]
        public ActionResult AlterarPrazo(CaptacaoPrazoRequest request)
        {
            var captacao = Service.Get(request.Id);
            captacao.Termino = request.Termino;
            Service.Put(captacao);
            return Ok();
        }
    }
}