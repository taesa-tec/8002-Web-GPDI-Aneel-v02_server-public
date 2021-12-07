using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeD.Authorizations;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Fornecedores;
using PeD.Core.Models.Projetos;
using PeD.Core.Requests.Projetos;
using PeD.Data;
using PeD.Services;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using Empresa = PeD.Core.Models.Projetos.Empresa;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos/{id:int}/RegistroFinanceiro")]
    [ApiController]
    [Authorize("Bearer")]
    public class RegistroFinanceiroController : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;
        private GestorDbContext _context;
        private ILogger<RegistroFinanceiroController> _logger;

        public RegistroFinanceiroController(ProjetoService service, IMapper mapper,
            IService<Core.Models.Empresa> serviceEmpresa,
            GestorDbContext context, ILogger<RegistroFinanceiroController> logger) :
            base(service, mapper)
        {
            Service = service;
            _context = context;
            _logger = logger;
        }

        protected async Task<bool> HasAccess(int projetoId)
        {
            var projeto = _context.Set<Projeto>().Find(projetoId);
            if (User.IsInRole(Roles.Administrador) || this.UserId() == projeto.ResponsavelId)
            {
                return true;
            }

            if (User.IsInRole(Roles.Fornecedor))
            {
                return await _context.Set<Fornecedor>()
                    .AnyAsync(f =>
                        f.ResponsavelId == this.UserId() && f.Id == projeto.FornecedorId); //Projeto.FornecedorId
            }

            return false;
        }

        [HttpGet("Extrato")]
        public async Task<ActionResult> GetExtrato(int id)
        {
            if (!await HasAccess(id))
                return Forbid();

            var extrato = Service.GetExtrato(id);
            return Ok(extrato);
        }

        [HttpGet("Criar")]
        public async Task<ActionResult> GetCriar([FromRoute] int id)
        {
            if (!await HasAccess(id))
                return Forbid();
            var colaboradores = Mapper.Map<List<RecursoHumanoDto>>(Service.NodeList<RecursoHumano>(id));
            var recursos = Mapper.Map<List<RecursoMaterialDto>>(Service.NodeList<RecursoMaterial>(id));
            var etapas = Mapper.Map<List<EtapaDto>>(Service.NodeList<Etapa>(id));
            var empresas = Mapper.Map<List<EmpresaDto>>(Service.NodeList<Empresa>(id));
            var projeto = Service.Get(id);
            var mesesN = etapas.SelectMany(etapa => etapa.Meses).Distinct();
            var meses = mesesN.Select(m => projeto.DataInicioProjeto.AddMonths(m - 1));
            var categorias = _context.CategoriasContabeis.ToList();
            return Ok(new
            {
                recursos, colaboradores, etapas, meses,
                empresas, categorias
            });
        }


        #region Listagem

        protected List<RegistroFinanceiroInfo> GetRefpStatus(int id, StatusRegistro status)
        {
            var isGestor = this.IsAdmin() || User.IsInRole(Roles.User);
            return _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == id && r.Status == status && (isGestor || r.AuthorId == this.UserId()))
                .ToList();
        }

        [HttpGet("Pendentes")]
        public ActionResult GetPendentes([FromRoute] int id)
        {
            var registros = GetRefpStatus(id, StatusRegistro.Pendente);
            return Ok(registros);
        }

        [HttpGet("Reprovados")]
        public ActionResult GetReprovados([FromRoute] int id)
        {
            var registros = GetRefpStatus(id, StatusRegistro.Reprovado);
            return Ok(registros);
        }

        [HttpGet("Aprovados")]
        public ActionResult GetAprovados([FromRoute] int id)
        {
            var registros = GetRefpStatus(id, StatusRegistro.Aprovado);
            return Ok(Mapper.Map<List<RegistroFinanceiroInfoDto>>(registros));
        }

        #endregion

        #region Criar Registro

        [HttpPost("RecursoHumano")]
        public async Task<ActionResult> CriarRh([FromRoute] int id, RegistroRhRequest request)
        {
            if (!await HasAccess(id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.FinanciadoraId && e.ProjetoId == id))
                return Forbid();

            try
            {
                var valorhora = _context.Set<RecursoHumano>()
                    .Where(r => r.Id == request.RecursoHumanoId)
                    .Select(r => r.ValorHora).First();
                var registro = Mapper.Map<RegistroFinanceiroRh>(request);
                registro.Valor = valorhora;
                registro.ProjetoId = id;
                registro.AuthorId = this.UserId();
                _context.Add(registro);
                _context.SaveChanges();

                if (!string.IsNullOrWhiteSpace(request.ObservacaoInterna))
                {
                    var obs = new RegistroObservacao()
                    {
                        Content = request.ObservacaoInterna,
                        AuthorId = this.UserId(),
                        CreatedAt = DateTime.Now,
                        RegistroId = registro.Id
                    };
                    _context.Add(obs);
                    _context.SaveChanges();
                }


                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                _logger.LogError("Erro na criação do registro: {Error}", e.Message);
                return Problem("Erro na criação do registro");
            }
        }

        [HttpPost("RecursoMaterial")]
        public async Task<ActionResult> CriarRm([FromRoute] int id, RegistroRmRequest request)
        {
            if (!await HasAccess(id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.FinanciadoraId && e.ProjetoId == id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.RecebedoraId && e.ProjetoId == id))
                return Forbid();
            try
            {
                var registro = Mapper.Map<RegistroFinanceiroRm>(request);
                registro.ProjetoId = id;
                registro.AuthorId = this.UserId();
                _context.Add(registro);
                _context.SaveChanges();

                if (!string.IsNullOrWhiteSpace(request.ObservacaoInterna))
                {
                    var obs = new RegistroObservacao()
                    {
                        Content = request.ObservacaoInterna,
                        AuthorId = this.UserId(),
                        CreatedAt = DateTime.Now,
                        RegistroId = registro.Id
                    };
                    _context.Add(obs);
                    _context.SaveChanges();
                }

                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                _logger.LogError("Erro na criação do registro: {Error}", e.Message);
                return Problem("Erro na criação do registro");
            }
        }

        #endregion

        #region Editar Registro

        [HttpPut("RecursoHumano/{registroId:int}")]
        public async Task<ActionResult> EditarRh([FromRoute] int id, [FromRoute] int registroId,
            RegistroRhRequest request)
        {
            if (!await HasAccess(id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.FinanciadoraId && e.ProjetoId == id))
                return Forbid();
            if (string.IsNullOrWhiteSpace(request.ObservacaoInterna))
            {
                return BadRequest();
            }

            try
            {
                var pre = _context.Set<RegistroFinanceiroRh>().AsNoTracking().FirstOrDefault(r =>
                    r.Id == registroId && (this.IsGestor() || r.AuthorId == this.UserId()));
                if (pre is null)
                    return NotFound();

                if (pre.ProjetoId != id)
                    return BadRequest();

                var valorhora = _context.Set<RecursoHumano>()
                    .Where(r => r.Id == request.RecursoHumanoId)
                    .Select(r => r.ValorHora).First();

                var registro = Mapper.Map<RegistroFinanceiroRh>(request);
                registro.Valor = valorhora;
                registro.ProjetoId = id;
                registro.AuthorId = pre.AuthorId;
                registro.Id = registroId;

                registro.ComprovanteId = _context.Set<RegistroFinanceiro>().Where(r => r.Id == registroId)
                    .Select(r => r.ComprovanteId).FirstOrDefault();
                _context.Update(registro);
                _context.SaveChanges();

                var obs = new RegistroObservacao()
                {
                    Content = request.ObservacaoInterna,
                    AuthorId = this.UserId(),
                    CreatedAt = DateTime.Now,
                    RegistroId = registro.Id
                };
                _context.Add(obs);
                _context.SaveChanges();
                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                _logger.LogError("Erro na atualização do registro: {Error}", e.Message);
                return Problem("Erro na atualização do registro");
            }
        }

        [HttpPut("RecursoMaterial/{registroId:int}")]
        public async Task<ActionResult> EditarRm([FromRoute] int id, [FromRoute] int registroId,
            RegistroRmRequest request)
        {
            if (!await HasAccess(id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.FinanciadoraId && e.ProjetoId == id) ||
                !_context.Set<Empresa>().Any(e => e.Id == request.RecebedoraId && e.ProjetoId == id)
            )
                return Forbid();
            if (string.IsNullOrWhiteSpace(request.ObservacaoInterna))
                return BadRequest();
            try
            {
                var pre = _context.Set<RegistroFinanceiroRm>().AsNoTracking()
                    .FirstOrDefault(r => r.Id == registroId && r.ProjetoId == id);

                if (pre is null)
                    return NotFound();

                var registro = Mapper.Map<RegistroFinanceiroRm>(request);
                registro.ProjetoId = id;
                registro.Id = registroId;
                registro.AuthorId = pre.AuthorId;
                registro.ComprovanteId = _context.Set<RegistroFinanceiro>().Where(r => r.Id == registroId)
                    .Select(r => r.ComprovanteId).FirstOrDefault();
                _context.Update(registro);
                _context.SaveChanges();

                var obs = new RegistroObservacao()
                {
                    Content = request.ObservacaoInterna,
                    AuthorId = this.UserId(),
                    CreatedAt = DateTime.Now,
                    RegistroId = registro.Id
                };
                _context.Add(obs);
                _context.SaveChanges();

                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                _logger.LogError("Erro na atualização do registro: {Error}", e.Message);
                return Problem("Erro na atualização do registro");
            }
        }

        [HttpDelete("{registroId:int}")]
        public async Task<ActionResult> Remover([FromRoute] int id, [FromRoute] int registroId)
        {
            if (!await HasAccess(id))
                return Forbid();
            var pre = _context.Set<RegistroFinanceiro>().Include(r => r.Observacoes).AsNoTracking()
                .FirstOrDefault(r => r.Id == registroId && r.ProjetoId == id);
            if (pre is null)
                return NotFound();
            if (this.IsGestor() || pre.AuthorId == this.UserId() && pre.Status == StatusRegistro.Pendente)
            {
                _context.RemoveRange(pre.Observacoes);
                _context.Remove(pre);
                _context.SaveChanges();
                return NoContent();
            }

            return Forbid();
        }

        #endregion

        #region Comprovante

        [HttpPost("{registroId:int}/Comprovante")]
        public async Task<ActionResult> UploadComprovante(int registroId, [FromServices] ArquivoService arquivoService)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo comprobatório não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            var registro = _context.Set<RegistroFinanceiro>()
                .FirstOrDefault(r => r.Id == registroId && (this.IsGestor() || r.AuthorId == this.UserId()));
            if (registro is null)
                return NotFound();
            try
            {
                var file = await arquivoService.SaveFile(upload);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE ProjetosRegistrosFinanceiros SET ComprovanteId = {0} WHERE Id = {1}",
                    file.Id, registroId);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError("Erro na atualização do registro: {Error}", e.Message);
                return Problem("Erro na atualização do registro");
            }
        }

        [HttpGet("{registroId:int}/Comprovante")]
        public async Task<ActionResult> DownloadComprovante(int registroId,
            [FromServices] ArquivoService arquivoService)
        {
            var registro = _context.Set<RegistroFinanceiro>().Include(r => r.Comprovante)
                .FirstOrDefault(r => r.Id == registroId);
            if (registro is null)
                return NotFound();

            if (!await HasAccess(registro.ProjetoId))
                return Forbid();

            var file = registro.Comprovante;
            return PhysicalFile(file.Path, file.ContentType, file.FileName);
        }

        #endregion

        #region Obter

        [HttpGet("{registroId:int}")]
        public async Task<ActionResult> Get(int registroId)
        {
            var registro = _context.Set<RegistroFinanceiro>().FirstOrDefault(r => r.Id == registroId);
            if (registro == null)
                return NotFound();
            if (!await HasAccess(registro.ProjetoId))
                return Forbid();
            if (registro.Tipo == "RegistroFinanceiroRh")
            {
                var rh = _context.Set<RegistroFinanceiroRh>()
                    .Include(r => r.RecursoHumano)
                    .ThenInclude(r => r.Empresa)
                    .Include(r => r.Etapa)
                    .Include(r => r.Financiadora)
                    .FirstOrDefault(r => r.Id == registroId && (this.IsGestor() || r.AuthorId == this.UserId()));
                return Ok(Mapper.Map<RegistroFinanceiroDto>(rh));
            }

            var rm = _context.Set<RegistroFinanceiroRm>().FirstOrDefault(r =>
                r.Id == registroId && (this.IsGestor() || r.AuthorId == this.UserId()));
            return Ok(Mapper.Map<RegistroFinanceiroDto>(rm));
        }

        [HttpGet("{registroId:int}/Info")]
        public async Task<ActionResult> GetInfo(int registroId)
        {
            var registro = _context.Set<RegistroFinanceiroInfo>().FirstOrDefault(r =>
                r.Id == registroId && (this.IsGestor() || r.AuthorId == this.UserId()));

            if (registro == null)
                return NotFound();
            if (!await HasAccess(registro.ProjetoId))
                return Forbid();
            return Ok(Mapper.Map<RegistroFinanceiroInfoDto>(registro));
        }

        [HttpGet("{registroId:int}/Observacoes")]
        public ActionResult GetObservacoes(int registroId)
        {
            var registro = _context.Set<RegistroFinanceiro>().FirstOrDefault(r => r.Id == registroId);

            if (registro != null && (this.IsGestor() || registro.AuthorId == this.UserId()))
            {
                var registroObservacoes = _context.Set<RegistroObservacao>().Include(ro => ro.Author)
                    .Where(ro => ro.RegistroId == registroId);

                return Ok(Mapper.Map<List<RegistroObservacaoDto>>(registroObservacoes));
            }

            return NotFound();
        }

        #endregion

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{registroId:int}/Aprovar")]
        public async Task<ActionResult> AprovarRegistroFinanceiro(int registroId)
        {
            var registro = _context.Set<RegistroFinanceiro>().FirstOrDefault(r => r.Id == registroId);
            if (registro is null)
            {
                return NotFound();
            }

            if (!await HasAccess(registro.ProjetoId))
                return Forbid();

            registro.Status = StatusRegistro.Aprovado;
            _context.Update(registro);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize(Policy = Policies.IsUserPeD)]
        [HttpPost("{registroId:int}/Reprovar")]
        public async Task<ActionResult> ReprovarRegistroFinanceiro(int registroId,
            [FromBody] RegistroAprovacaoRequest request)
        {
            var registro = _context.Set<RegistroFinanceiro>().FirstOrDefault(r => r.Id == registroId);
            if (registro is null)
            {
                return NotFound();
            }

            if (!await HasAccess(registro.ProjetoId))
                return Forbid();
            if (string.IsNullOrWhiteSpace(request.Observacao))
            {
                return BadRequest("Observação não pode ser vazia");
            }

            if (registro.Status == StatusRegistro.Pendente)
            {
                registro.Status = StatusRegistro.Reprovado;
                var obs = new RegistroObservacao()
                {
                    Content = request.Observacao,
                    RegistroId = registro.Id,
                    AuthorId = this.UserId(),
                    CreatedAt = DateTime.Now
                };
                _context.Add(obs);
                _context.Update(registro);
                _context.SaveChanges();
                return Ok();
            }

            return Conflict();
        }
    }
}