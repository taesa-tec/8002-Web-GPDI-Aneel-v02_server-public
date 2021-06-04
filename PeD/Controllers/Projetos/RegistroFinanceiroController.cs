using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
using PeD.Core.Models.Projetos;
using PeD.Core.Requests.Projetos;
using PeD.Data;
using PeD.Services;
using PeD.Services.Projetos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Projetos
{
    [SwaggerTag("Projeto")]
    [Route("api/Projetos/{id:int}/RegistroFinanceiro")]
    [ApiController]
    [Authorize("Bearer")]
    public class RegistroFinanceiroController : ControllerServiceBase<Projeto>
    {
        private new ProjetoService Service;
        private IService<Empresa> _serviceEmpresa;
        private GestorDbContext _context;

        public RegistroFinanceiroController(ProjetoService service, IMapper mapper, IService<Empresa> serviceEmpresa,
            GestorDbContext context) :
            base(service, mapper)
        {
            Service = service;
            _serviceEmpresa = serviceEmpresa;
            _context = context;
        }

        //[ResponseCache(Duration = 3600)]
        [HttpGet("Criar")]
        public ActionResult GetCriar([FromRoute] int id)
        {
            var colaboradores = Mapper.Map<List<RecursoHumanoDto>>(Service.NodeList<RecursoHumano>(id));
            var recursos = Mapper.Map<List<RecursoMaterialDto>>(Service.NodeList<RecursoMaterial>(id));
            var etapas = Mapper.Map<List<EtapaDto>>(Service.NodeList<Etapa>(id));
            var coexecutores = Mapper.Map<List<CoExecutorDto>>(Service.NodeList<CoExecutor>(id));
            var projeto = Service.Get(id);
            var empresas = _serviceEmpresa.Filter(q => q
                .Where(e => e.Categoria == Empresa.CategoriaEmpresa.Taesa || e.Id == projeto.FornecedorId));
            var mesesN = etapas.SelectMany(etapa => etapa.Meses).Distinct();
            var meses = mesesN.Select(m => projeto.DataInicioProjeto.AddMonths(m - 1));
            var categorias = _context.CategoriasContabeis.ToList();
            return Ok(new
            {
                recursos, colaboradores, etapas, meses, coexecutores, empresas, categorias
            });
        }

        [HttpPost("RecursoHumano")]
        public ActionResult CriarRh([FromRoute] int id, RegistroRhRequest request)
        {
            try
            {
                var valorhora = _context.Set<RecursoHumano>()
                    .Where(r => r.Id == request.RecursoHumanoId)
                    .Select(r => r.ValorHora).First();
                var registro = Mapper.Map<RegistroFinanceiroRh>(request);
                registro.Valor = valorhora;
                registro.ProjetoId = id;
                _context.Add(registro);
                _context.SaveChanges();

                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem(e.Message);
            }
        }

        [HttpPost("RecursoMaterial")]
        public ActionResult CriarRm([FromRoute] int id, RegistroRmRequest request)
        {
            try
            {
                var registro = Mapper.Map<RegistroFinanceiroRm>(request);
                registro.ProjetoId = id;
                _context.Add(registro);
                _context.SaveChanges();
                return Ok(Mapper.Map<RegistroFinanceiroDto>(registro));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem(e.Message);
            }
        }

        [HttpPost("{registroId:int}/Comprovante")]
        public async Task<ActionResult> UploadComprovante(int registroId, [FromServices] ArquivoService arquivoService)
        {
            var upload = Request.Form.Files.FirstOrDefault();
            if (upload is null)
            {
                return Problem("O arquivo comprobatório não foi enviado", null,
                    StatusCodes.Status422UnprocessableEntity);
            }

            try
            {
                var file = await arquivoService.SaveFile(upload);
                _context.Database.ExecuteSqlRaw(
                    "UPDATE ProjetosRegistrosFinanceiros SET ComprovanteId = {0} WHERE Id = {1} AND ComprovanteId IS NULL",
                    file.Id, registroId);
                return Ok();
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [HttpGet("Pendentes")]
        public ActionResult GetPendentes([FromRoute] int id)
        {
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == id && r.Status == StatusRegistro.Pendente)
                .ToList();
            return Ok(registros);
        }

        [HttpGet("Reprovados")]
        public ActionResult GetReprovados([FromRoute] int id)
        {
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == id && r.Status == StatusRegistro.Reprovado)
                .ToList();
            return Ok(registros);
        }

        [HttpGet("Aprovados")]
        public ActionResult GetAprovados([FromRoute] int id)
        {
            var registros = _context.Set<RegistroFinanceiroInfo>()
                .Where(r => r.ProjetoId == id && r.Status == StatusRegistro.Aprovado)
                .ToList();
            return Ok(Mapper.Map<List<RegistroFinanceiroInfoDto>>(registros));
        }

        [HttpGet("{registroId:int}")]
        public ActionResult Get(int registroId)
        {
            var registro = _context.Set<RegistroFinanceiroInfo>().FirstOrDefault(r => r.Id == registroId);

            if (registro == null)
                return NotFound();
            return Ok(Mapper.Map<RegistroFinanceiroInfoDto>(registro));
        }

        protected RegistroFinanceiroRh GetRegistroRh(int id)
        {
            return _context.Set<RegistroFinanceiroRh>().FirstOrDefault(r => r.Id == id);
        }

        protected RegistroFinanceiroRm GetRegistroRm(int id)
        {
            return _context.Set<RegistroFinanceiroRm>().FirstOrDefault(r => r.Id == id);
        }
    }
}