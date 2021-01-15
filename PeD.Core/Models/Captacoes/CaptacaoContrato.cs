using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Captacoes
{
    public class CaptacaoContrato
    {
        public int CaptacaoId { get; set; }
        public int ContratoId { get; set; }
        [ForeignKey("CaptacaoId")] public Captacao Captacao { get; set; }

        [ForeignKey("ContratoId")] public Contrato Contrato { get; set; }
    }
}