using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers
{
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class EmpresasController : ControllerBase
    {
        private EmpresaService _service;

        public EmpresasController(EmpresaService service)
        {
            _service = service;
        }

        [HttpGet("{projetoId}/Empresas")]
        public IEnumerable<Empresa> Get(int projetoId)
        {
            return _service.ListarTodos(projetoId);
        }

        [Route("[controller]")]
        [HttpPost]
        public Resultado Post([FromBody]Empresa Empresa)
        {
            return _service.Incluir(Empresa);
        }

        [Route("[controller]")]
        [HttpPut]
        public Resultado Put([FromBody]Empresa Empresa)
        {
            return _service.Atualizar(Empresa);
        }

        [HttpDelete("[controller]/{Id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}