using Microsoft.Extensions.DependencyInjection;
using PeD.Core.Models;

namespace PeD.Authorizations
{
    public static class RoleAuthorizations
    {
        public static void AddRoleAuthorizations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.IsAdmin, policy => policy.RequireRole(Roles.Administrador));
                options.AddPolicy(Policies.IsUserPeD, policy => policy.RequireRole(Roles.Administrador, Roles.User));
                options.AddPolicy(Policies.IsUserTaesa,
                    policy => policy.RequireRole(Roles.Administrador, Roles.User, Roles.Suprimento));
                options.AddPolicy(Policies.IsColaborador,
                    policy => policy.RequireRole(Roles.Administrador, Roles.User, Roles.Suprimento, Roles.Colaborador));
                options.AddPolicy(Policies.IsSuprimento, policy => policy.RequireRole(Roles.Suprimento));
                options.AddPolicy(Policies.IsFornecedor, policy => policy.RequireRole(Roles.Fornecedor));
            });
        }
    }
}