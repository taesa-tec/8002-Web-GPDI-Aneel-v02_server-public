namespace APIGestor.Models.Captacao
{
    public class PropostaConfiguracaoArquivos : FileUpload
    {
        public int PropostaConfiguracaoId { get; set; }
        public PropostaConfiguracao PropostaConfiguracao { get; set; }
    }
}