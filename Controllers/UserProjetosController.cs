using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using APIGestor.Business;
using APIGestor.Models;
using System.Linq;

namespace APIGestor.Controllers
{
    [ApiController]
    [Authorize("Bearer")]

    public class UserProjetosController : ControllerBase
    {
        private UserProjetoService _service;

        public UserManager<ApplicationUser> UserManager {get; private set;}

        public UserProjetosController(UserProjetoService service)
        {
            _service = service;
        }

        //[Route("api/UserProjetos")]
        [HttpGet("api/UserProjetos/me")]
        public IEnumerable<UserProjeto> Get()
        {
            var user = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            if (user != null)
                return _service.ListarTodos(user.ToString());
            else
                return null;
        }

        //[Route("api/UserProjetos")]
        [HttpGet("api/UserProjetos/{userId}")]
        public IEnumerable<UserProjeto> GetA(string userId)
        {
            if (userId != null)
                return _service.ListarTodos(userId);
            else
                return null;
        }

        [Route("api/UserProjetos")]
        [HttpPost]
        public Resultado Post([FromBody]List<UserProjeto> UserProjeto)
        {
            return _service.Incluir(UserProjeto);
        }

        [Route("api/ProjetoUsers")]
        [HttpPost]
        public Resultado PostA([FromBody]List<UserProjeto> UserProjeto)
        {
            return _service.IncluirProjeto(UserProjeto);
        }

        [Route("api/UserProjetos")]
        [HttpDelete("{projetoId}")]
        public Resultado Delete(int projetoId)
        {
            return _service.Excluir(projetoId);
        }
    }
}