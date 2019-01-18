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
    [Route("api/UserProjetos")]
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


        [HttpGet("me")]
        public IEnumerable<Projeto> Get()
        {
            var user = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;

            if (user != null)
                return _service.ListarTodos(user.ToString());
            else
                return null;
        }

        [HttpGet("{userId}")]
        public IEnumerable<Projeto> GetA(string userId)
        {
            if (userId != null)
                return _service.ListarTodos(userId);
            else
                return null;
        }

        [HttpPost]
        public Resultado Post([FromBody]UserProjeto UserProjeto)
        {
            return _service.Incluir(UserProjeto);
        }

        [HttpPut]
        public Resultado Put([FromBody]UserProjeto UserProjeto)
        {
            return _service.Atualizar(UserProjeto);
        }

        [HttpDelete("{projetoId}")]
        public Resultado Delete(int projetoId)
        {
            return _service.Excluir(projetoId);
        }
    }
}