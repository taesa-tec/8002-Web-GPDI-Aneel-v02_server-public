using APIGestor.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using APIGestor.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace APIGestor.Controllers
{
    [SwaggerTag("Login")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public object Post(
            [FromBody] Login user,
            [FromServices] AccessManager accessManager)
        {
            // @todo Validar o status retornado 
            if (accessManager.ValidateCredentials(user))
            {
                return accessManager.GenerateToken(user);
            }
            else
            {
                return new
                {
                    Authenticated = false,
                    Message = "Falha ao autenticar"
                };
            }
        }

        [HttpPost("recuperar-senha")]
        public object PostA(
            [FromBody] Login user,
            [FromServices] AccessManager accessManager)
        {
            return accessManager.RecuperarSenha(user);
        }

        [HttpPost("nova-senha")]
        public object PostB(
            [FromBody] User user,
            [FromServices] AccessManager accessManager)
        {
            return accessManager.NovaSenha(user);
        }
    }
}