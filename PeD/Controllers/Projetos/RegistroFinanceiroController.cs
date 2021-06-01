using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.ApiModels.Projetos;
using PeD.Core.Models;
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
            return Ok(new
            {
                recursos, colaboradores, etapas, meses, coexecutores, empresas
            });
        }

        [HttpPost("RecursoHumano")]
        public ActionResult CriarRh([FromRoute] int id, RegistroRhRequest request)
        {
            try
            {
                var registro = Mapper.Map<RegistroFinanceiroRh>(request);
                registro.ProjetoId = id;
                _context.Add(registro);
                _context.SaveChanges();
                return Ok();
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
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Problem(e.Message);
            }
        }
    }
}