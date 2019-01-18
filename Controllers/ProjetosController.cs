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
    public class ProjetosController : ControllerBase
    {
        private ProjetoService _service;

        public ProjetosController(ProjetoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Projeto> Get()
        {
            return _service.ListarTodos();
        }

        [HttpGet("{id}")]
        public ActionResult<Projeto> Get(int id)
        {
            var Projeto = _service.Obter(id);
            if (Projeto != null)
                return Projeto;
            else
                return NotFound();
        }
        
        [HttpGet("{id}/Usuarios")]
        public IEnumerable<UserProjeto> GetA(int id)
        {
            return _service.ObterUsuarios(id);
        }

        [HttpPost]
        public Resultado Post([FromBody]Projeto Projeto)
        {
            return _service.Incluir(Projeto);
        }

        [HttpPut]
        public Resultado Put([FromBody]Projeto Projeto)
        {
            return _service.Atualizar(Projeto);
        }

        [HttpDelete("{id}")]
        public Resultado Delete(int id)
        {
            return _service.Excluir(id);
        }
    }
}