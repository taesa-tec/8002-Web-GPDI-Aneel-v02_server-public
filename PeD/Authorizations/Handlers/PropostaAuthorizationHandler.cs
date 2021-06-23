using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PeD.Authorizations.Requirements;
using PeD.Core.Models;
using PeD.Core.Models.Captacoes;
using PeD.Core.Models.Propostas;
using PeD.Data;

namespace PeD.Authorizations.Handlers
{
    public class PropostaAuthorizationHandler : AuthorizationHandler<PropostaRequirement, Proposta>
    {
        private IServiceProvider _services;

        public PropostaAuthorizationHandler(IServiceProvider services)
        {
            _services = services;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PropostaRequirement requirement, Proposta resource)
        {
            var userId = context.User.FindFirst(JwtRegisteredClaimNames.Jti).Value;
            var scope = _services.CreateScope();
            var dbcontext = scope.ServiceProvider.GetRequiredService<GestorDbContext>();
            var captacao = dbcontext.Set<Captacao>().FirstOrDefault(c => c.Id == resource.CaptacaoId);

            if (userId == resource.ResponsavelId)
            {
                if (resource.Participacao == StatusParticipacao.Rejeitado)
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                if (!requirement.CanEdit || requirement.CanEdit && captacao != null &&
                    (captacao.Status == Captacao.CaptacaoStatus.Fornecedor ||
                     captacao.Status == Captacao.CaptacaoStatus.Refinamento))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                if (context.User.IsInRole(Roles.Administrador) || captacao != null && new[]
                        {captacao.UsuarioAprovacaoId, captacao.UsuarioExecucaoId, captacao.UsuarioRefinamentoId}
                    .Contains(userId))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }


            return Task.CompletedTask;
        }
    }
}