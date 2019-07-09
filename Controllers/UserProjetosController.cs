using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using APIGestor.Business;
using APIGestor.Models;
using System.Linq;

namespace APIGestor.Controllers {
    [ApiController]
    [Authorize("Bearer")]

    public class UserProjetosController : ControllerBase {
        private UserProjetoService _service;

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public UserProjetosController( UserProjetoService service ) {
            _service = service;
        }

        //[Route("api/UserProjetos")]
        [HttpGet("api/UserProjetos/me")]
        public List<Projeto> Get() {
            if(this.isAdmin()) {
                return _service.projetoService.ListarTodos();
            }
            else {
                return _service.ListarTodos(this.userId()).Select(up => up.Projeto).ToList();
            }
        }

        //[Route("api/UserProjetos")]
        [HttpGet("api/UserProjetos/{userId}")]
        public IEnumerable<UserProjeto> GetA( string userId ) {
            if(userId != null)
                return _service.ListarTodos(userId);
            else
                return null;
        }

        [Route("api/UserProjetos")]
        [HttpPost]
        public ActionResult<Resultado> Post( [FromBody]List<UserProjeto> UserProjeto ) {
            var pids = UserProjeto.GroupBy(p => p.ProjetoId).Select(p => p.Key).ToList();
            var auth = true;

            // Garantir que, caso o usuário atual tenha acesso administrativo a um dos projeto, não altere projetos que ele não tenha acesso
            foreach(int id in pids) {
                auth = auth && (_service.UserProjectCan(id, User, Authorizations.ProjectPermissions.Administrator));
            }

            if(auth) {
                return _service.Incluir(UserProjeto);
            }
            return Forbid();
        }

        [Route("api/ProjetoUsers")]
        [HttpPost]
        public ActionResult<Resultado> PostA( [FromBody]List<UserProjeto> UserProjeto ) {
            var pids = UserProjeto.GroupBy(p => p.ProjetoId).Select(p => p.Key).ToList();
            var auth = true;

            foreach(int id in pids) {
                auth = auth && (_service.UserProjectCan(id, User, Authorizations.ProjectPermissions.Administrator));
            }
            if(auth) {
                return _service.IncluirProjeto(UserProjeto);
            }
            return Forbid();
        }

        [Route("api/UserProjetos")]
        [HttpDelete("{projetoId}")]
        public ActionResult<Resultado> Delete( int projetoId ) {
            return _service.Excluir(projetoId);
        }
    }
}