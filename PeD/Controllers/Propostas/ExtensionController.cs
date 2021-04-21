using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PeD.Auth;
using PeD.Authorizations;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

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

        public static async Task<AuthorizationResult> Authorize<T>(this PropostaNodeBaseController<T> controller,
            Proposta proposta,
            string policyName = PropostaAuthorizations.Access) where T : BaseEntity
        {
            return await controller.AuthorizationService.AuthorizeAsync(controller.User, proposta, policyName);
        }
    }
}