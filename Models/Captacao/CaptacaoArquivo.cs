
namespace APIGestor.Models.Captacao
{
    public class CaptacaoArquivo : FileUpload
    {
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
    }
}