using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PeD.Core.Models;


namespace PeD.Core.Extensions
{
    public static class ControllerExtension
    {
        public static string UserId(this ControllerBase controller)
        {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static bool IsAdmin(this ControllerBase controller)
        {
            return controller.User.FindFirst(ClaimTypes.Role).Value == Roles.AdminGestor;
        }
    }
}