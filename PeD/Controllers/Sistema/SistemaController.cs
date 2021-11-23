using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeD.Authorizations;
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

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(Empresa model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(Empresa model)
        {
            return base.Put(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
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

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(Estado model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(Estado model)
        {
            return base.Put(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
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

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Post(Pais model)
        {
            return base.Post(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Put(Pais model)
        {
            return base.Put(model);
        }

        [Authorize(Policy = Policies.IsAdmin)]
        public override IActionResult Delete(int id)
        {
            return base.Delete(id);
        }
    }
}