using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PeD.Core.Models.Projetos;

namespace PeD.Core.Authorizations
{
    public class ProjectAuthorizationHandler : AuthorizationHandler<ProjectAccessRequirement, UserProjeto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ProjectAccessRequirement requirement, UserProjeto resource)
        {
            var userPermission = resource.CatalogUserPermissao.Valor;
            if (ProjectPermissions.NiveisValues.ContainsKey(userPermission) &&
                (ProjectPermissions.NiveisValues[userPermission] & requirement.AccessValue) == requirement.AccessValue)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

    public class ProjectAccessRequirement : IAuthorizationRequirement
    {
        public readonly int AccessValue;

        public ProjectAccessRequirement(int AccessValue)
        {
            this.AccessValue = AccessValue;
        }
    }

    public static class ProjectPermissions
    {
        public enum Access
        {
            Leitura = 1,
            Escrita = 2,
            Aprovador = 4,
            Administrador = 8
        }

        public static Dictionary<string, int> NiveisValues = new Dictionary<string, int>
        {
            {"leitura", (int) Access.Leitura},
            {"leituraEscrita", (int) (Access.Leitura | Access.Escrita)},
            {"aprovador", (int) (Access.Leitura | Access.Escrita | Access.Aprovador)},
            {"admin", (int) (Access.Leitura | Access.Escrita | Access.Aprovador | Access.Administrador)}
        };

        public static ProjectAccessRequirement Leitura = new ProjectAccessRequirement((int) (Access.Leitura));

        public static ProjectAccessRequirement LeituraEscrita =
            new ProjectAccessRequirement((int) (Access.Leitura | Access.Escrita));

        public static ProjectAccessRequirement Aprovador =
            new ProjectAccessRequirement((int) (Access.Leitura | Access.Escrita | Access.Aprovador));

        public static ProjectAccessRequirement Administrator =
            new ProjectAccessRequirement((int) (Access.Leitura | Access.Escrita | Access.Aprovador |
                                                Access.Administrador));
    }
}