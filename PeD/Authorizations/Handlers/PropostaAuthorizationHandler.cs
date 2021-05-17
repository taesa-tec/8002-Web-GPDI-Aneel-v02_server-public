using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PeD.Authorizations.Requirements;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;

namespace PeD.Authorizations.Handlers
{
    public class PropostaAuthorizationHandler : AuthorizationHandler<PropostaRequirement, Proposta>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PropostaRequirement requirement, Proposta resource)
        {
            if (
                resource.Participacao != StatusParticipacao.Rejeitado &&
                context.User.FindFirst(JwtRegisteredClaimNames.Jti).Value == resource.ResponsavelId
            )
            {
                context.Succeed(requirement);
            }
            else if (!requirement.CaptacaoEncerrada || resource.Captacao?.Status >= Captacao.CaptacaoStatus.Encerrada)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}