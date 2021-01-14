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
using PeD.Core.Models.Captacao;
using PeD.Core.Requests.Captacao;
using PeD.Data;
using PeD.Services;
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

        public CaptacaoController(IService<Captacao> service, IMapper mapper, UserManager<ApplicationUser> userManager,
            IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor,
            IService<CaptacaoInfo> serviceInfo)
            : base(service, mapper)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
            _userManager = userManager;
            _serviceInfo = serviceInfo;
        }

        [HttpGet("")]
        public ActionResult GetCaptacoes()
        {
            var captacoes = Service.Filter(q =>
                q.Include(c => c.UsuarioSuprimento)
                    .Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao &&
                                c.UsuarioSuprimentoId == this.userId()));
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [HttpGet("Pendentes")]
        public ActionResult<List<CaptacaoPendenteDto>> GetPendentes()
        {
            //Service.Paged()
            var captacoes = Service.Filter(q =>
                q.Include(c => c.Criador).Where(c => c.Status == Captacao.CaptacaoStatus.Pendente));
            var mapped = Mapper.Map<List<CaptacaoPendenteDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Elaboracao")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetEmElaboracao()
        {
            //Service.Paged()
            var captacoes = Service.Filter(q =>
                q.Include(c => c.UsuarioSuprimento).Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao));
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [HttpGet("Canceladas")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetCanceladas()
        {
            //Service.Paged()
            var captacoes =
                Service.Filter(q =>
                    q.Where(c => c.Status == Captacao.CaptacaoStatus.Cancelada)); // ou com zero propostas
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
            var captacoes =
                Service.Filter(q =>
                    q.Where(c =>
                        c.Status == Captacao.CaptacaoStatus.Fornecedor &&
                        c.Termino <= DateTime.Today));
            var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(mapped);
        }


        [HttpPost("NovaCaptacao")]
        public async Task<ActionResult> NovaCaptacao(NovaCaptacaoRequest request,
            [FromServices] GestorDbContext context,
            [FromServices] SendGridService sendGridService,
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

            var nova = new NovaCaptacao()
            {
                Autor = currentUser.NomeCompleto,
                CaptacaoId = captacao.Id,
                CaptacaoTitulo = captacao.Titulo
            };
            await sendGridService.Send("diego.franca@lojainterativa.com", "Novo Projeto para captação",
                "Email/Captacao/NovaCaptacao", nova);
            return Ok();
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

        // @todo Authorization GetCaptacao
        [HttpGet("{id}")]
        public ActionResult<CaptacaoDetalhesDto> GetCaptacao(int id)
        {
            var captacao = Service.Filter(q => q
                .Include(c => c.Arquivos)
                .Include(c => c.FornecedoresSugeridos)
                .ThenInclude(fs => fs.Fornecedor)
                .Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao &&
                            c.UsuarioSuprimentoId == this.userId() &&
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
    }
}