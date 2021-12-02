using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using PeD.Core.Models;

namespace PeD.Controllers
{
    public static class GestorControllerExtension
    {
        public static string UserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static bool IsAdmin(this ControllerBase controller)
        {
            return controller.User.IsInRole(Roles.Administrador);
        }

        public static bool IsGestor(this ControllerBase controller)
        {
            return controller.User.IsInRole(Roles.Administrador) || controller.User.IsInRole(Roles.User);
        }
    }
}