using Microsoft.AspNetCore.Authorization;
using PeD.Core.Models.Propostas;

namespace PeD.Authorizations.Requirements
{
    public class PropostaRequirement : IAuthorizationRequirement
    {
        public bool CanEdit { get; }
        public bool CaptacaoEncerrada { get; }


        public PropostaRequirement(bool captacaoEncerrada = false, bool canEdit = false)
        {
            CaptacaoEncerrada = captacaoEncerrada;
            CanEdit = canEdit;
        }
    }
}