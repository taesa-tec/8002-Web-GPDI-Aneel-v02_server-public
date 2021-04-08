using System;
using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class CaptacaoPrazoRequest : BaseEntity
    {
        public DateTime Termino { get; set; }
    }

    public class CaptacaoSelecaoPropostaRequest
    {
        public int PropostaId { get; set; }
        public string UsuarioRefinamentoId { get; set; }
        public DateTime DataAlvo { get; set; }
    }
}