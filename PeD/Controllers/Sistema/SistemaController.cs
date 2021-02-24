using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeD.Core.Models;
using PeD.Core.Models.Catalogos;
using Swashbuckle.AspNetCore.Annotations;
using TaesaCore.Controllers;
using TaesaCore.Interfaces;

namespace PeD.Controllers.Sistema
{
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