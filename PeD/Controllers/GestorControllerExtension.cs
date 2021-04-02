using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PeD.Core.Models;

namespace PeD.Controllers
{
    public static class GestorControllerExtension
    {
        public static string UserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static int UserEmpresaId(this ControllerBase controller)
        {
            var id = controller.User.FindFirst(ClaimTypes.GroupSid)?.Value ?? "0";
            return int.Parse(id);
        }

        public static bool IsAdmin(this ControllerBase controller)
        {
            return controller.User.FindFirst(ClaimTypes.Role).Value == Roles.Administrador;
        }
    }
}