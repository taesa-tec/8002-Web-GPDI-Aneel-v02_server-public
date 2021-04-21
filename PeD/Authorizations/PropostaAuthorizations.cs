using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PeD.Authorizations.Handlers;
using PeD.Authorizations.Requirements;
using PeD.Core.Models.Propostas;

namespace PeD.Authorizations
{
    public static class PropostaAuthorizations
    {
        public const string Access = "AccessPropostaPolicy";
        public const string AccessEncerradas = "AccessPropostaEncerradaPolicy";
        public const string Edit = "EditPropostaPolicy";

        public static void AddPropostaAuthorizations(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Access, policy => policy.Requirements.Add(new PropostaRequirement()));
                options.AddPolicy(AccessEncerradas,
                    policy => policy.Requirements.Add(
                        new PropostaRequirement(captacaoEncerrada: true)));
                options.AddPolicy(Edit, policy => policy.Requirements.Add(new PropostaRequirement(canEdit: true)));
            });
            services.AddSingleton<IAuthorizationHandler, PropostaAuthorizationHandler>();
        }
    }
}