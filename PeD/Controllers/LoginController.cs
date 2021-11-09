using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Net.Http.Headers;
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
            [FromServices] AccessManager accessManager,
            [FromServices] IDistributedCache cache)
        {
            if (accessManager.ValidateCredentials(user))
            {
                var token = accessManager.GenerateToken(user);
                cache.Set(token.AccessToken,
                    Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                    new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(4))
                );
                return token;
            }

            return Problem("Usuário ou senha incorreto", null, StatusCodes.Status401Unauthorized);
        }

        [AllowAnonymous]
        [HttpGet("/api/logout")]
        [HttpPost("/api/logout")]
        public ActionResult Logout([FromServices] IDistributedCache cache)
        {
            if (Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                var headerAuthorization = Request.Headers[HeaderNames.Authorization].Single().Split(" ").Last();
                cache.Remove(headerAuthorization);
            }

            return Ok();
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