using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Requests.Captacao;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using PeD.Views.Email.Captacao;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using TaesaCore.Models;

namespace PeD.Controllers.Captacoes
{
    [SwaggerTag("Captacao")]
    [Route("api/Captacoes")]
    [ApiController]
    [Authorize("Bearer")]
    public class CaptacaoController : ControllerServiceBase<Captacao>
    {
        private UserManager<ApplicationUser> _userManager;
        private IUrlHelper _urlHelper;
        private IService<CaptacaoInfo> _serviceInfo;
        private new CaptacaoService Service;

        public CaptacaoController(CaptacaoService service, IMapper mapper, UserManager<ApplicationUser> userManager,
            IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,
            IService<CaptacaoInfo> serviceInfo)
            : base(service, mapper)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _userManager = userManager;
            _serviceInfo = serviceInfo;
            Service = service;
        }

        [HttpGet("")]
        public ActionResult GetCaptacoes()
        {
            var captacoes = Service.Filter(q =>
                q.Include(c => c.UsuarioSuprimento)
                    .Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao &&
                                c.UsuarioSuprimentoId == this.UserId()));
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Pendentes")]
        public ActionResult<List<CaptacaoPendenteDto>> GetPendentes()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoes(Captacao.CaptacaoStatus.Pendente);
            var mapped = Mapper.Map<List<CaptacaoPendenteDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Elaboracao")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetEmElaboracao()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoes(Captacao.CaptacaoStatus.Elaboracao);
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Canceladas")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetCanceladas()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesFalhas();
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Abertas")]
        public ActionResult<List<CaptacaoInfo>> GetAbertas()
        {
            //Service.Paged()
            var captacoes =
                _serviceInfo.Filter(q =>
                    q.Where(c =>
                        c.Status == Captacao.CaptacaoStatus.Fornecedor &&
                        c.Termino > DateTime.Today));
            // var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(captacoes);
        }

        [HttpGet("Encerradas")]
        public ActionResult<List<CaptacaoDto>> GetEncerradas()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesEncerradas();
                
            var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(mapped);
        }

        // @todo Authorization GetCaptacao
        [HttpGet("{id}")]
        public ActionResult<CaptacaoDetalhesDto> GetCaptacao(int id)
        {
            var captacao = Service.Filter(q => q
                .Include(c => c.Arquivos)
                .Include(c => c.FornecedoresSugeridos)
                .ThenInclude(fs => fs.Fornecedor)
                .Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao &&
                            c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();
            if (captacao == null)
            {
                return NotFound();
            }

            var detalhes = Mapper.Map<CaptacaoDetalhesDto>(captacao);
            detalhes.EspecificacaoTecnicaUrl = _urlHelper.Link("DemandaPdf",
                new {id = captacao.DemandaId, form = "especificacao-tecnica"});

            return Ok(detalhes);
        }

        [HttpPost("NovaCaptacao")]
        public async Task<ActionResult> NovaCaptacao(NovaCaptacaoRequest request,
            [FromServices] GestorDbContext context,
            [FromServices] IService<Contrato> contratoService)
        {
            var captacao = Service.Get(request.Id);
            if (captacao.Status == Captacao.CaptacaoStatus.Elaboracao && captacao.EnvioCaptacao != null)
            {
                return BadRequest(new {error = "Captação já está em elaboração"});
            }

            if (!contratoService.Exist(request.ContratoId))
            {
                return BadRequest(new {error = "Contrato sugerido não existe ou foi removido"});
            }

            captacao.Observacoes = request.Observacoes;
            captacao.EnvioCaptacao = DateTime.Now;
            captacao.Status = Captacao.CaptacaoStatus.Elaboracao;
            captacao.ContratoSugeridoId = request.ContratoId;
            captacao.UsuarioSuprimentoId = request.UsuarioSuprimentoId;
            Service.Put(captacao);
            if (request.Fornecedores.Count > 0)
            {
                var fornecedoresSugeridos = request.Fornecedores.Select(fid => new CaptacaoSugestaoFornecedor()
                {
                    CaptacaoId = request.Id,
                    FornecedorId = fid
                });

                var dbset = context.Set<CaptacaoSugestaoFornecedor>();
                dbset.RemoveRange(dbset.Where(csf => csf.CaptacaoId == request.Id));
                context.SaveChanges();
                await dbset.AddRangeAsync(fornecedoresSugeridos);
                context.SaveChanges();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            await Service.SendEmailSuprimento(captacao, currentUser.NomeCompleto);
            return Ok();
        }

        [HttpPut("Cancelar")]
        public async Task<ActionResult> Cancelar(BaseEntity request)
        {
            try
            {
                await Service.CancelarCaptacao(request.Id);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPut("AlterarPrazo")]
        public async Task<ActionResult> AlterarPrazo(CaptacaoPrazoRequest request)
        {
            try
            {
                await Service.EstenderCaptacao(request.Id, request.Termino);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }
    }
}