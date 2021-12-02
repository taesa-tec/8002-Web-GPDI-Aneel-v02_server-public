using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Authorizations;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Projetos;
using PeD.Core.Models.Projetos.Xml;
using PeD.Core.Requests.Projetos;
using PeD.Data;
using PeD.Services;
using PeD.Services.Projetos;
using PeD.Services.Projetos.Xml;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;
using Empresa = PeD.Core.Models.Projetos.Empresa;
using Log = Serilog.Log;

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

        private IQueryable<Projeto> _projetoQuery(IQueryable<Projeto> q)
        {
            var isGestor = this.IsAdmin() || User.IsInRole(Roles.User);
            return q.Include(p => p.Fornecedor)
                .Where(p => isGestor || p.Fornecedor.ResponsavelId == this.UserId());
        }

        public ProjetoController(ProjetoService service, IMapper mapper, GestorDbContext context) : base(service,
            mapper)
        {
            Service = service;
            Context = context;
        }

        private bool CanAccess(int id)
        {
            return Service.Filter(q =>
                _projetoQuery(q).Where(p => p.Id == id)).Any();
        }

        protected IList<Projeto> GetProjetosStatus(Status status)
        {
            return Service.Filter(q => _projetoQuery(q)
                .Where(p => p.Status == status));
        }

        [HttpGet("EmExecucao")]
        public ActionResult GetEmExecucao()
        {
            var projetos = GetProjetosStatus(Status.Execucao);
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("Finalizados")]
        public ActionResult GetFinalizados()
        {
            var projetos = GetProjetosStatus(Status.Finalizado);
            return Ok(Mapper.Map<List<ProjetoDto>>(projetos));
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            var projeto = Service.Filter(q =>
                _projetoQuery(q)
                    .Include(p => p.Proponente)
                    .Include(p => p.Fornecedor)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return Ok(Mapper.Map<ProjetoDto>(projeto));
        }

        [HttpPut("{id:int}/Status")]
        public ActionResult UpdateStatus(int id, [FromBody] ProjetoStatusRequest request)
        {
            var projeto = Service.Filter(q => _projetoQuery(q)
                .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();
            if (projeto.Status == request.Status)
                return Problem($"O projeto já está no com status {projeto.Status.ToString()}", null,
                    StatusCodes.Status400BadRequest);

            projeto.Status = request.Status;
            Context.Update(projeto);
            Context.SaveChanges();
            if (projeto.Status == Status.Finalizado)
                Service.RelatoriosEtapas(projeto.Id);

            return Ok(Mapper.Map<ProjetoDto>(projeto));
        }

        [HttpGet("{id:int}/PlanoTrabalho")]
        public ActionResult GetPlanoTrabalho(int id)
        {
            var projeto = Service.Filter(q =>
                _projetoQuery(q).Include(p => p.PlanoTrabalhoFile)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return PhysicalFile(projeto.PlanoTrabalhoFile.Path, projeto.PlanoTrabalhoFile.ContentType,
                projeto.PlanoTrabalhoFile.FileName);
        }

        [HttpGet("{id:int}/Contrato")]
        public ActionResult GetContrato(int id)
        {
            var projeto = Service.Filter(q =>
                _projetoQuery(q).Include(p => p.Contrato)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return PhysicalFile(projeto.Contrato.Path, projeto.Contrato.ContentType,
                projeto.Contrato.FileName);
        }

        [HttpGet("{id:int}/EspecificacaoTecnica")]
        public ActionResult GetEspecificacaoTecnica(int id)
        {
            var projeto = Service.Filter(q =>
                _projetoQuery(q).Include(p => p.EspecificacaoTecnicaFile)
                    .Where(p => p.Id == id)).FirstOrDefault();
            if (projeto == null)
                return NotFound();

            return PhysicalFile(projeto.EspecificacaoTecnicaFile.Path, projeto.EspecificacaoTecnicaFile.ContentType,
                projeto.EspecificacaoTecnicaFile.FileName);
        }

        [HttpGet("{id:int}/Empresas")]
        public ActionResult Empresas(int id, [FromServices] IService<Core.Models.Empresa> empresasService)
        {
            if (CanAccess(id))
            {
                var empresas = Context.Set<Empresa>().AsQueryable().Where(c => c.ProjetoId == id).ToList();
                return Ok(empresas);
            }

            return NotFound();
        }

        [HttpGet("{id:int}/Produtos")]
        public ActionResult Produtos(int id)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var produtos = Context.Set<Produto>()
                .Where(p => p.ProjetoId == id)
                .ToList();

            return Ok(Mapper.Map<List<ProjetoProdutoDto>>(produtos));
        }

        [Authorize(Policy = Policies.IsUserPeD)]
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
            for (var i = 1; i <= etapaMesesTotal; i++)
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
                Ordem = (short)ordem
            };
            Context.Add(etapa);
            Context.Update(projeto);
            Context.SaveChanges();
            return Ok();
        }

        [HttpGet("{projetoId}/ExtratoFinanceiro/Xlsx")]
        public ActionResult GetXlsx([FromRoute] int projetoId)
        {
            if (!CanAccess(projetoId))
            {
                return NotFound();
            }

            var xls = Service.XlsExtrato(projetoId);
            var stream = new MemoryStream();
            xls.SaveAs(stream);
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"projeto-{projetoId}-relatorio.xlsx");
        }

        #region Logs DUTO

        [HttpGet("{id:int}/LogsDuto")]
        public ActionResult GetLogsDuto(int id, [FromServices] IService<ProjetoXml> service)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var logs = service.Filter(q => q
                .Include(l => l.File)
                .Where(l => l.ProjetoId == id && l.Tipo == BaseXml.XmlTipo.DUTO));
            return Ok(Mapper.Map<List<ProjetoXmlDto>>(logs));
        }

        [HttpGet("{id:int}/Xml/{xmlId:int}")]
        public ActionResult GetLogDuto(int id, int xmlId, [FromServices] IService<ProjetoXml> service)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var log = service.Filter(q => q
                .Include(l => l.File)
                .Where(l => l.ProjetoId == id && l.Id == xmlId)).FirstOrDefault();
            if (log is null)
                return NotFound();

            return PhysicalFile(log.File.Path, log.File.ContentType, log.File.FileName);
        }

        [HttpPost("{id:int}/LogsDuto")]
        public async Task<ActionResult> UploadComprovante([FromRoute] int id,
            [FromServices] ArquivoService arquivoService,
            [FromServices] IService<ProjetoXml> serviceXml)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            if (!upload.FileName.ToLower().EndsWith(".xml", true, null))
            {
                return BadRequest("É necessário enviar um arquivo xml");
            }

            try
            {
                var file = await arquivoService.SaveFile(upload);
                var xml = new ProjetoXml()
                {
                    FileId = file.Id,
                    ProjetoId = id,
                    Tipo = BaseXml.XmlTipo.DUTO,
                    Versao = "1"
                };
                serviceXml.Post(xml);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        #endregion

        #region XMLS

        [HttpGet("{id:int}/Xmls")]
        public ActionResult GetXmls(int id, [FromServices] IService<ProjetoXml> service)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var logs = service.Filter(q => q
                .Include(l => l.File)
                .Where(l => l.ProjetoId == id && l.Tipo != BaseXml.XmlTipo.DUTO));
            return Ok(Mapper.Map<List<ProjetoXmlDto>>(logs));
        }

        #endregion

        #region XML PRORROGACAO

        [HttpPost("{id:int}/GerarXML/Prorrogacao")]
        public ActionResult GerarXmlProrrogacao(int id, [FromBody] XmlRequest request)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var projeto = Service.Get(id);
            if (projeto is null)
                return NotFound();
            try
            {
                var doc = Service.SaveXml(id, request.Versao, new Prorrogacao(projeto.Codigo, projeto.Duracao));
                return PhysicalFile(doc.File.Path, doc.File.ContentType, doc.File.FileName);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("{id:int}/GerarXML/FinalProjeto")]
        public ActionResult GerarXmlFinalProjeto(int id, [FromBody] XmlRequest request,
            [FromServices] RelatorioFinalService service)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var projeto = Service.Get(id);
            if (projeto is null)
                return NotFound();


            try
            {
                var doc = Service.SaveXml(id, request.Versao, service.RelatorioFinalPeD(id));
                return PhysicalFile(doc.File.Path, doc.File.ContentType, doc.File.FileName);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpPost("{id:int}/GerarXML/Auditoria")]
        public ActionResult GerarXmlAuditoria(int id, [FromBody] XmlRequest request,
            [FromServices] RelatorioAuditoriaService service)
        {
            if (!CanAccess(id))
            {
                return NotFound();
            }

            var projeto = Service.Get(id);
            if (projeto is null)
                return NotFound();

            try
            {
                var doc = Service.SaveXml(id, request.Versao, service.Auditoria(id));
                return PhysicalFile(doc.File.Path, doc.File.ContentType, doc.File.FileName);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error:{Error}", e.Message);
                return Problem(e.Message);
            }
        }

        #endregion
    }
}