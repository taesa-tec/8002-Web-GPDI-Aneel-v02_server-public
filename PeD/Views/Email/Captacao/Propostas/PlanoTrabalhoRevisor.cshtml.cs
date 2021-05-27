using System;

namespace PeD.Views.Email.Captacao.Propostas
{
    public class PlanoTrabalhoRevisor
    {
        public Guid PropostaGuid { get; set; }
        public Core.Models.Captacoes.Captacao Captacao { get; set; }
    }
}