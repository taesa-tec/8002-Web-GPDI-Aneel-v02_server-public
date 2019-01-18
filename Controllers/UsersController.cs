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
    public class UsersController : ControllerBase
    {
        private UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            return _service.ListarTodos();
        }

        [HttpGet("{id}")]
        public ActionResult<ApplicationUser> Get(string id)
        {
            var User = _service.Obter(id);
            if (User != null)
                return User;
            else
                return NotFound();
        }

        [HttpGet("me")]
        public ActionResult<ApplicationUser> GetA()
        {
            var user = User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            if (user != null)
                return  _service.Obter(user.ToString());
            else
                return NotFound();
        }

        [HttpPost]
        public Resultado Post([FromBody]User User)
        {
            return _service.Incluir(User);
        }

        [HttpPut]
        public Resultado Put([FromBody]ApplicationUser User)
        {
            return _service.Atualizar(User);
        }

        [HttpDelete("{id}")]
        public Resultado Delete(string id)
        {
            return _service.Excluir(id);
        }
    }
}