namespace APIGestor.Models.Captacao
{
    public class CaptacaoContrato
    {
        public int CaptacaoId { get; set; }
        public int ContratoId { get; set; }
        public Captacao Captacao { get; set; }
        public Contrato Contrato { get; set; }
    }
}