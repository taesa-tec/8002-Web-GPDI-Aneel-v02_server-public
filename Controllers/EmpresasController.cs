using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class EmpresasController : ControllerBase
    {
        private EmpresaService _service;

        public EmpresasController(EmpresaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Empresa> Get()
        {
            return _service.ListarTodos();
        }

        [HttpGet("{id}")]
        public ActionResult<Empresa> Get(int id)
        {
            var Empresa = _service.Obter(id);
            if (Empresa != null)
                return Empresa;
            else
                return NotFound();
        }

        [HttpPost]
        public Resultado Post([FromBody]Empresa Empresa)
        {
            return _service.Incluir(Empresa);
        }

        [HttpPut]
        public Resultado Put([FromBody]Empresa Empresa)
        {
            return _service.Atualizar(Empresa);
        }

        [HttpDelete("{id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}