using System;

namespace PeD.Views.Email.Captacao.Propostas
{
    public class RefinamentoConcluido
    {
        public Guid PropostaGuid { get; set; }
        public Core.Models.Captacoes.Captacao Captacao { get; set; }
    }
}