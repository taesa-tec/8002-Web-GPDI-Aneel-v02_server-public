using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PeD.Core.Models;

namespace PeD.Controllers
{
    public static class GestorControllerExtension
    {
        public static string userId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static bool isAdmin(this ControllerBase controller)
        {
            return controller.User.FindFirst(ClaimTypes.Role).Value == Roles.AdminGestor;
        }
    }
}