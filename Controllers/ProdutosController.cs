using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Business;
using APIGestor.Models;

namespace APIGestor.Controllers {
    [Route("api/projeto/")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProdutosController : ControllerBase {
        private ProdutoService _service;

        public ProdutosController( ProdutoService service ) {
            _service = service;
        }

        [HttpGet("{projetoId}/Produtos")]
        public ActionResult<List<Produto>> Get( int projetoId ) {
            if(this._service.UserProjectCan(projetoId, User)) {
                return _service.ListarTodos(projetoId);
            }
            return Forbid();
        }

        [HttpGet("Produto/{id}")]
        public ActionResult<Produto> GetA( int id ) {

            var Produto = _service.Obter(id);
            if(Produto != null) {
                if(this._service.UserProjectCan(Produto.ProjetoId, User)) {
                    return Produto;
                }
                return Forbid();
            }
            else
                return NotFound();
        }

        [Route("[controller]")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]Produto Produto ) {
            if(this._service.UserProjectCan(Produto.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Incluir(Produto);
            }
            return Forbid();
        }

        [Route("[controller]")]
        [HttpPut]
        public ActionResult<Resultado> Put( [FromBody]Produto Produto ) {
            if(this._service.UserProjectCan(Produto.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                return _service.Atualizar(Produto);
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            Produto p = _service.Obter(id);
            if(this._service.UserProjectCan(p.ProjetoId, User, Authorizations.ProjectPermissions.Administrator)) {
                return _service.Excluir(id);
            }
            return Forbid();
        }
    }
}