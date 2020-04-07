using System;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoCanceladaDto : BaseEntity
    {
        public DateTime Termino { get; set; }
        public DateTime Cancelamento { get; set; }
        public string Status { get; set; }
        public int ConvidadosTotal { get; set; }
        public int PropostaTotal { get; set; }
    }
}