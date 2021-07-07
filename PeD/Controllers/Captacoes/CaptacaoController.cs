using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using PeD.Authorizations;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.ApiModels.Propostas;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Requests.Captacao;
using PeD.Data;
using PeD.Services;
using PeD.Services.Captacoes;
using PeD.Services.Projetos;
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

        #region 2.2

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("")]
        public ActionResult GetCaptacoes()
        {
            var captacoes = Service.Filter(q =>
                q.Include(c => c.UsuarioSuprimento)
                    .Where(c => c.Status == Captacao.CaptacaoStatus.Elaboracao &&
                                c.UsuarioSuprimentoId == this.UserId()));
            return Ok(Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes));
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Pendentes")]
        public ActionResult<List<CaptacaoPendenteDto>> GetPendentes()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoes(Captacao.CaptacaoStatus.Pendente);
            var mapped = Mapper.Map<List<CaptacaoPendenteDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Elaboracao")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetEmElaboracao()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoes(Captacao.CaptacaoStatus.Elaboracao);
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Canceladas")]
        public ActionResult<List<CaptacaoElaboracaoDto>> GetCanceladas()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesFalhas();
            var mapped = Mapper.Map<List<CaptacaoElaboracaoDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
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

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Encerradas")]
        public ActionResult<List<CaptacaoDto>> GetEncerradas()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesEncerradas();

            var mapped = Mapper.Map<List<CaptacaoDto>>(captacoes);
            return Ok(mapped);
        }


        [Authorize(Policy = Policies.IsUserTaesa)]
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

        #endregion

        #region 2.3

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("SelecaoPendente")]
        public ActionResult<List<CaptacaoDto>> GetSelecaoPendente()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesSelecaoPendente();

            var mapped = Mapper.Map<List<CaptacaoSelecaoPendenteDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("SelecaoFinalizada")]
        public ActionResult<List<CaptacaoDto>> GetSelecaoFinalizada()
        {
            //Service.Paged()
            var captacoes = Service.GetCaptacoesSelecaoFinalizada();

            var mapped = Mapper.Map<List<CaptacaoSelecaoFinalizadaDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/Propostas")]
        public ActionResult<List<PropostaSelecaoDto>> GetCaptacaoPropostas(int id,
            [FromServices] PropostaService propostaService)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            //c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();
            if (captacao == null)
            {
                return NotFound();
            }

            var propostas = propostaService.Filter(q =>
                q.Include(p => p.Contrato)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.Finalizado && p.CaptacaoId == id && p.Contrato != null));

            return Mapper.Map<List<PropostaSelecaoDto>>(propostas);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/Propostas/{propostaId}/PlanoTrabalho")]
        public ActionResult DownloadPlanoTrabalho(int id, int propostaId,
            [FromServices] PropostaService propostaService)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            //c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();

            var proposta = propostaService.Filter(q =>
                q.Include(p => p.Relatorio)
                    .ThenInclude(r => r.File)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.CaptacaoId == id && p.Finalizado && p.Id == propostaId)).FirstOrDefault();
            if (captacao == null || proposta == null || proposta.Relatorio?.File == null)
            {
                return NotFound();
            }

            var file = proposta.Relatorio.File;
            return PhysicalFile(file.Path, file.ContentType,
                $"{captacao.Titulo}-plano-de-trabalho({proposta.Fornecedor.Nome}).pdf");
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/Propostas/{propostaId}/Contrato")]
        public ActionResult DownloadContrato(int id, int propostaId,
            [FromServices] PropostaService propostaService)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            //c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();

            var proposta = propostaService.Filter(q =>
                q.Include(p => p.Contrato)
                    .ThenInclude(r => r.File)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.CaptacaoId == id && p.Finalizado && p.Id == propostaId)).FirstOrDefault();
            if (captacao == null || proposta == null || proposta.Contrato?.File == null)
            {
                return NotFound();
            }

            var file = proposta.Contrato.File;
            return PhysicalFile(file.Path, file.ContentType,
                $"{captacao.Titulo}-contrato({proposta.Fornecedor.Nome}).pdf");
        }


        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/PropostaSelecionada/PlanoTrabalho")]
        public ActionResult DownloadPlanoTrabalhoPropostaSelecionada(int id,
            [FromServices] PropostaService propostaService)
        {
            var captacao = Service.Get(id);
            if (captacao == null || captacao.PropostaSelecionadaId == null)
            {
                return NotFound();
            }

            return DownloadPlanoTrabalho(id, captacao.PropostaSelecionadaId.Value, propostaService);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/PropostaSelecionada/Contrato")]
        public ActionResult DownloadContratoPropostaSelecionada(int id,
            [FromServices] PropostaService propostaService)
        {
            var captacao = Service.Get(id);
            if (captacao == null || captacao.PropostaSelecionadaId == null)
            {
                return NotFound();
            }

            return DownloadContrato(id, captacao.PropostaSelecionadaId.Value, propostaService);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/SelecionarProposta")]
        public ActionResult SelecionarProposta(int id, [FromBody] CaptacaoSelecaoRequest request)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            //c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            if (request.DataAlvo < DateTime.Today)
            {
                return Problem("Data alvo não pode ser menor que hoje", null, StatusCodes.Status409Conflict);
            }

            captacao.DataAlvo = request.DataAlvo;
            captacao.UsuarioRefinamentoId = request.ResponsavelId;
            captacao.PropostaSelecionadaId = request.PropostaId;
            captacao.Status = Captacao.CaptacaoStatus.Refinamento;
            Service.Put(captacao);
            Service.SendEmailSelecao(captacao).Wait();
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/SelecionarProposta/Arquivo")]
        public async Task<ActionResult> ArquivoProbatorio(int id, [FromServices] ArquivoService arquivoService)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo comprobatório não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            //c.UsuarioSuprimentoId == this.UserId() &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            var file = await arquivoService.SaveFile(upload);
            captacao.ArquivoComprobatorioId = file.Id;
            Service.Put(captacao);
            return Ok();
        }

        #endregion

        #region 2.4

        //[Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Refinamento")]
        public ActionResult GetPropostasRefinamento()
        {
            var propostas =
                Service.GetPropostasRefinamento(this.IsAdmin() ? "" : this.UserId(), User.IsInRole(Roles.Fornecedor));
            return Ok(Mapper.Map<List<PropostaDto>>(propostas));
        }

        #endregion

        #region 2.5

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("IdentificaoRiscoPendente")]
        public ActionResult<List<CaptacaoDto>> GetIdentificaoRiscoPendente()
        {
            //Service.Paged()
            var captacoes = Service.GetIdentificaoRiscoPendente(this.IsAdmin() ? null : this.UserId());

            var mapped = Mapper.Map<List<CaptacaoIdentificaoRiscosDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("IdentificaoRiscoFinalizada")]
        public ActionResult<List<CaptacaoDto>> IdentificaoRiscoFinalizada()
        {
            //Service.Paged()
            var captacoes = Service.GetIdentificaoRiscoFinalizada();

            var mapped = Mapper.Map<List<CaptacaoIdentificaoRiscosDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/ConfirmarRiscos")]
        public ActionResult ConfirmarRiscos(int id, [FromBody] CaptacaoIdentificaoRiscoRequest request)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status == Captacao.CaptacaoStatus.AnaliseRisco &&
                            (c.UsuarioRefinamentoId == this.UserId() || this.IsAdmin()) &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(request.ResponsavelId))
            {
                return BadRequest();
            }

            captacao.UsuarioAprovacaoId = request.ResponsavelId;
            captacao.Status = Captacao.CaptacaoStatus.Formalizacao;
            Service.Put(captacao);
            Service.SendEmailFormalizacaoPendente(captacao).Wait();
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/ConfirmarRiscos/Arquivo")]
        public async Task<ActionResult> ArquivoRiscos(int id, [FromServices] ArquivoService arquivoService)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo comprobatório não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            var captacao = Service.Filter(q => q
                .Where(c => c.Status >= Captacao.CaptacaoStatus.Encerrada &&
                            (c.UsuarioRefinamentoId == this.UserId() || this.IsAdmin()) &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            var file = await arquivoService.SaveFile(upload);
            captacao.ArquivoRiscosId = file.Id;
            Service.Put(captacao);
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/IdentificaoRisco/Arquivo")]
        public ActionResult DownloadIdentificaoRisco(int id)
        {
            var captacao = Service.Filter(q => q
                .Include(c => c.ArquivoRiscos)
                .Where(c => c.Status >= Captacao.CaptacaoStatus.AnaliseRisco &&
                            c.Id == id
                )).FirstOrDefault();
            if (captacao is null)
                return NotFound();
            var file = captacao.ArquivoRiscos;
            return PhysicalFile(file.Path, file.ContentType,
                $"{captacao.Titulo}-riscos.pdf");
        }

        #endregion

        #region 2.6

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("FormalizacaoPendente")]
        public ActionResult<List<CaptacaoFormalizacaoDto>> GetFormalizacaoPendente()
        {
            //Service.Paged()
            var captacoes = Service.GetFormalizacao(null, this.IsAdmin() ? null : this.UserId());

            var mapped = Mapper.Map<List<CaptacaoFormalizacaoDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("Formalizados")]
        public ActionResult<List<CaptacaoFormalizacaoDto>> Formalizados()
        {
            //Service.Paged()
            var captacoes = Service.GetFormalizacao(true, this.IsAdmin() ? null : this.UserId());

            var mapped = Mapper.Map<List<CaptacaoFormalizacaoDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("NoDeal")]
        public ActionResult<List<CaptacaoFormalizacaoDto>> NoDeal()
        {
            //Service.Paged()
            var captacoes = Service.GetFormalizacao(false);

            var mapped = Mapper.Map<List<CaptacaoFormalizacaoDto>>(captacoes);
            return Ok(mapped);
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/Formalizacao")]
        public ActionResult Formalizar(int id, [FromBody] CaptacaoFormalizacaoRequest request,
            [FromServices] ProjetoService projetoService)
        {
            var captacao = Service.Filter(q => q
                .Where(c => c.Status == Captacao.CaptacaoStatus.Formalizacao &&
                            (c.UsuarioAprovacaoId == this.UserId() || this.IsAdmin()) &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            if (request.Aprovado &&
                (
                    string.IsNullOrWhiteSpace(request.ResponsavelId) ||
                    !request.InicioProjeto.HasValue ||
                    !request.EmpresaProponenteId.HasValue
                )
            )
            {
                return BadRequest();
            }

            if (captacao.PropostaSelecionadaId == null)
                return Problem("Sem proposta selecionada");

            if (request.Aprovado)
            {
                captacao.UsuarioExecucaoId = request.ResponsavelId;
            }

            captacao.IsProjetoAprovado = request.Aprovado;
            Service.Put(captacao);

            if (request.Aprovado)
            {
                var projeto = projetoService.ParseProposta(captacao.PropostaSelecionadaId.Value,
                    request.EmpresaProponenteId.Value, request.NumeroProjeto,
                    request.TituloCompleto, request.ResponsavelId, request.InicioProjeto.Value);
            }


            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{id}/Formalizacao/Arquivo")]
        public async Task<ActionResult> ArquivoFormalizacao(int id, [FromServices] ArquivoService arquivoService)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo comprobatório não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            var captacao = Service.Filter(q => q
                .Where(c => c.Status == Captacao.CaptacaoStatus.Formalizacao &&
                            (c.UsuarioAprovacaoId == this.UserId() || this.IsAdmin()) &&
                            c.Id == id
                )).FirstOrDefault();

            if (captacao == null)
            {
                return NotFound();
            }

            var file = await arquivoService.SaveFile(upload);
            captacao.ArquivoFormalizacaoId = file.Id;
            Service.Put(captacao);
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpGet("{id}/Formalizacao/Arquivo")]
        public ActionResult DownloadFormalizacao(int id)
        {
            var captacao = Service.Filter(q => q
                .Include(c => c.ArquivoRiscos)
                .Where(c => c.Status >= Captacao.CaptacaoStatus.AnaliseRisco &&
                            c.Id == id
                )).FirstOrDefault();
            if (captacao is null)
                return NotFound();
            var file = captacao.ArquivoRiscos;
            return PhysicalFile(file.Path, file.ContentType,
                $"{captacao.Titulo}-riscos.pdf");
        }

        #endregion
    }
}