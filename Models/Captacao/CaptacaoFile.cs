namespace APIGestor.Models.Captacao
{
    public class CaptacaoFile : FileUpload
    {
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
    }
}