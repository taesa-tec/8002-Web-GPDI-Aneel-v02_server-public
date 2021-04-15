using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PeD.Auth;
using PeD.Authorizations;
using PeD.Core.Models.Propostas;

namespace PeD.Controllers.Propostas
{
    public static class ExtensionController
    {
        public static async Task<AuthorizationResult> Authorize(this PropostasController controller,
            Proposta proposta,
            string policyName = PropostaAuthorizations.Access)
        {
            return await controller.AuthorizationService.AuthorizeAsync(controller.User, proposta, policyName);
        }
    }
}