using System.Collections.Generic;
using APIGestor.Models.Projetos;
using APIGestor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGestor.Controllers.Projetos {
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

         // CONTROLLER
        [HttpPost("[controller]")]
        public ActionResult<Resultado> Post( [FromBody]Produto Produto ) {
            if(this._service.UserProjectCan(Produto.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var resultado = _service.Incluir(Produto);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, Produto.ProjetoId, Produto);
                }
                return resultado;
            }
            return Forbid();
        }

         // CONTROLLER
        [HttpPut("[controller]")]
        public ActionResult<Resultado> Put( [FromBody]Produto Produto ) {
            if(this._service.UserProjectCan(Produto.ProjetoId, User, Authorizations.ProjectPermissions.LeituraEscrita)) {
                var oldProduto = _service.Obter(Produto.Id);
                _service._context.Entry(oldProduto).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                var resultado = _service.Atualizar(Produto);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, oldProduto.ProjetoId, _service.Obter(Produto.Id), oldProduto);
                }
                return resultado;
            }
            return Forbid();
        }

        [HttpDelete("[controller]/{Id}")]
        public ActionResult<Resultado> Delete( int id ) {
            Produto p = _service.Obter(id);
            if(this._service.UserProjectCan(p.ProjetoId, User, Authorizations.ProjectPermissions.Administrator)) {
                var resultado = _service.Excluir(id);
                if(resultado.Sucesso) {
                    this.CreateLog(_service, p.ProjetoId, p);
                }

                return resultado;
            }
            return Forbid();
        }
    }
}