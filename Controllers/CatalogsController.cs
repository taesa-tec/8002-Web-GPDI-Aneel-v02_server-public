using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;
using APIGestor.Security;
using System.IdentityModel.Tokens.Jwt;

namespace APIGestor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class CatalogsController : ControllerBase
    {
        private CatalogService _service;

        public CatalogsController(CatalogService service)
        {
            _service = service;
        }

        [HttpGet("temas")]
        public IEnumerable<CatalogTema> Get()
        {
            return _service.ListarCatalogTema();
        }

        [HttpGet("status")]
        public IEnumerable<CatalogStatus> GetA()
        {
            return _service.ListarCatalogStatus();
        }

        [HttpGet("segmentos")]
        public IEnumerable<CatalogSegmento> GetB()
        {
            return _service.ListarCatalogSegmento();
        }

        [HttpGet("empresas")]
        public IEnumerable<CatalogEmpresa> GetC()
        {
            return _service.ListarCatalogEmpresa();
        }
        [HttpGet("estados")]
        public IEnumerable<CatalogEstado> GetD()
        {
            return _service.ListarCatalogEstados();
        }

        [HttpGet("permissoes")]
        public IEnumerable<CatalogUserPermissao> GetE()
        {
            return _service.ListarCatalogUserPermissao();
        }
    }
}