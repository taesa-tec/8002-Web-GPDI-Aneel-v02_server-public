using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Extensions;
using PeD.Core.Models;
using PeD.Core.Models.Catalogos;
using PeD.Core.Requests;
using PeD.Data;
using PeD.Services;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
    [Authorize]
    [Route("api/Sistema")]
    [ApiController]
    public class SistemaController : ControllerBase
    {
        private InstallService InstallService { get; }


        public SistemaController(InstallService installService)
        {
            InstallService = installService;
        }

        [AllowAnonymous]
        [HttpGet("Status")]
        public ActionResult GetStatus()
        {
            return Ok(new
            {
                InstallService.Installed
            });
        }

        [AllowAnonymous]
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
            catch (Exception)
            {
                return BadRequest("Ocorreu um erro ao criar o usuário administrativo");
            }
        }
    }

    [Authorize("Bearer")]
    [SwaggerTag("Empresas")]
    [Route("api/Empresas")]
    [ApiController]
    public class EmpresasController : ControllerCrudBase<Empresa>
    {
        private GestorDbContext _context;

        public EmpresasController(IService<Empresa> service, IMapper mapper, GestorDbContext context) :
            base(service, mapper)
        {
            _context = context;
        }

        [HttpGet]
        public override ActionResult<List<Empresa>> Get()
        {
            var user = _context.Users.First(u => u.Id == this.UserId());
            if (Roles.Fornecedor == user.Role)
            {
                var empresas = Service.Filter(q =>
                    q.Where(e => e.Id == user.EmpresaId || e.Categoria == Empresa.CategoriaEmpresa.Taesa));
                return Ok(empresas);
            }

            return base.Get();
        }
    }

    [SwaggerTag("Estados")]
    [Route("api/Estados")]
    [ApiController]
    public class EstadosController : ControllerCrudBase<Estado>
    {
        public EstadosController(IService<Estado> service, IMapper mapper) : base(service, mapper)
        {
        }
    }

    [SwaggerTag("Paises")]
    [Route("api/Paises")]
    [ApiController]
    public class PaisesController : ControllerCrudBase<Pais>
    {
        public PaisesController(IService<Pais> service, IMapper mapper) : base(service, mapper)
        {
        }
    }
}