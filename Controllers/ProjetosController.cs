using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using APIGestor.Business;
using APIGestor.Models;
using APIGestor.Data;
using System.Security.Claims;
using APIGestor.Security;
using APIGestor.Authorizations;
using Microsoft.EntityFrameworkCore;

namespace APIGestor.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class ProjetosController : ControllerBase {
        private ProjetoService _service;
        private UserProjetoService _userprojeto_service;
        protected GestorDbContext _context;

        public ProjetosController( ProjetoService service, GestorDbContext context, UserProjetoService userprojeto_service ) {
            _service = service;
            _context = context;
            _userprojeto_service = userprojeto_service;
        }

        [HttpGet]
        public IEnumerable<Projeto> Get() {

            if(this.isAdmin()) {
                return _service.ListarTodos();
            }

            return _userprojeto_service.ListarTodos(this.userId())
                .Select(item => item.Projeto);
        }

        [HttpGet("{id}")]
        public ActionResult<Projeto> Get( int id ) {
            var Projeto = _service.Obter(id);


            if(Projeto != null) {
                if(_service.UserProjectCan(id, User)) {

                    return Projeto;
                }
                return Forbid();
            }
            return NotFound();
        }
        [HttpGet("{id}/me")]
        public ActionResult<CatalogUserPermissao> myAccess( int id ) {
            if(this.isAdmin()) {
                return new CatalogUserPermissao { Nome = "Administrador", Valor = "admin" };
            }
            var userProjeto = _context.UserProjetos.Include("CatalogUserPermissao").Where(up => up.ProjetoId == id && up.UserId == this.userId()).FirstOrDefault();
            if(userProjeto != null) {
                return userProjeto.CatalogUserPermissao;
            }
            return null;
        }

        [HttpGet("{id}/Usuarios")]
        public ActionResult<List<UserProjeto>> GetA( int id ) {
            if(this._service.UserProjectCan(id, User, ProjectPermissions.Leitura))
                return _service.ObterUsuarios(id);
            return Forbid();
        }

        [HttpPost] // Criar
        public ActionResult<Resultado> Post( [FromBody]Projeto Projeto ) {
            if(this.isAdmin()) {
                return _service.Incluir(Projeto, this.userId());
            }
            return Forbid();
        }

        [HttpPut] // Editar
        public ActionResult<Resultado> Put( [FromBody]Projeto Projeto ) {
            if(this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
                return _service.Atualizar(Projeto);
            return Forbid();
        }

        [HttpDelete("{id}")] // Apagar
        public ActionResult<Resultado> Delete( int id ) {
            if(this._service.UserProjectCan(id, User, ProjectPermissions.Administrator))
                return _service.Excluir(id);
            return Forbid();
        }

        [HttpPut("dataInicio")]
        public ActionResult<Resultado> PutA( [FromBody]Projeto Projeto ) {
            if(this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
                return _service.AtualizaDataInicio(Projeto);
            return Forbid();
        }

        [HttpPost("prorrogar")]
        public object PostA( [FromBody]Projeto Projeto ) {
            if(this._service.UserProjectCan(Projeto.Id, User, ProjectPermissions.Administrator))
                return _service.ProrrogarProjeto(Projeto);
            return Forbid();
        }
    }
}