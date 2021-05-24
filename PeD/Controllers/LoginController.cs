using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PeD.Auth;
using PeD.Core.ApiModels.Auth;
using PeD.Core.Requests;
using PeD.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PeD.Controllers
{
    [SwaggerTag("Login")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Token> Post(
            [FromBody] Login user,
            [FromServices] AccessManager accessManager)
        {
            if (accessManager.ValidateCredentials(user))
            {
                return accessManager.GenerateToken(user);
            }

            return Problem("Usuário ou senha incorreto", null, StatusCodes.Status401Unauthorized);
        }

        [HttpPost("recuperar-senha")]
        public object PostA(
            [FromBody] Login user,
            [FromServices] AccessManager accessManager)
        {
            if (accessManager.RecuperarSenha(user))
            {
                return Ok();
            }

            return NotFound();
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