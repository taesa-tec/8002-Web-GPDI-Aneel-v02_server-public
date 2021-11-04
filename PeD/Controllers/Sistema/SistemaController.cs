using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;
using PeD.Core.Models.Catalogos;
using PeD.Core.Requests;
using PeD.Services;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [Route("api/Sistema")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private InstallService InstallService { get; }


        public SistemaController(InstallService installService)
        {
            InstallService = installService;
        }

        [HttpGet("Status")]
        public ActionResult GetStatus()
        {
            return Ok(new
            {
                InstallService.Installed
            });
        }

        [HttpPost("Install")]
        public async Task<ActionResult> Install(InstallRequest request)
        {
            if (InstallService.Installed)
                return NotFound();
            try
            {
                await InstallService.Install(request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Ocorreu um erro ao criar o usuário administrativo");
            }
        }
    }

    [SwaggerTag("Empresas")]
    [Route("api/Empresas")]
    [ApiController]
    [Authorize("Bearer")]
    public class EmpresasController : ControllerCrudBase<Empresa>
    {
        public EmpresasController(IService<Empresa> service, IMapper mapper) : base(service, mapper)
        {
        }
    }

    [SwaggerTag("Estados")]
    [Route("api/Estados")]
    [ApiController]
    [Authorize("Bearer")]
    public class EstadosController : ControllerCrudBase<Estado>
    {
        public EstadosController(IService<Estado> service, IMapper mapper) : base(service, mapper)
        {
        }
    }

    [SwaggerTag("Paises")]
    [Route("api/Paises")]
    [ApiController]
    [Authorize("Bearer")]
    public class PaisesController : ControllerCrudBase<Pais>
    {
        public PaisesController(IService<Pais> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}