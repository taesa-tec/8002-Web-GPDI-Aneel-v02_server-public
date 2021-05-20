using System;

namespace PeD.Views.Email.Captacao
{
    public class AlteracaoPrazo
    {
        public Guid PropostaGuid { get; set; }
        public string Projeto { get; set; }
        public DateTime Prazo { get; set; }
    }
}