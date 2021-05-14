using System;

namespace PeD.Views.Email.Captacao.Propostas
{
    public class RevisorConvite
    {
        public string Fornecedor { get; set; }
        public Guid PropostaGuid { get; set; }
        public DateTime DataAlvo { get; set; }
        public Core.Models.Captacoes.Captacao Captacao { get; set; }
    }
}