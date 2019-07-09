using APIGestor.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APIGestor.Controllers {
    public static class SecurityControllerExtension {

        public static string userId( this ControllerBase controller ) {
            return controller.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
        }

        public static bool isAdmin( this ControllerBase controller ) {
            return controller.User.FindFirst(ClaimTypes.Role).Value == Roles.ROLE_ADMIN_GESTOR;
        }
    }
}
