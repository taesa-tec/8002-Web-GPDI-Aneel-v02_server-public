using System;
using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class CaptacaoPrazoRequest : BaseEntity
    {
        public DateTime Termino { get; set; }
    }
}