namespace APIGestor.Models.Captacao
{
    public class PropostaConfiguracaoArquivo : FileUpload
    {
        public int CaptacaoPropostaConfiguracaoId { get; set; }
        public PropostaConfiguracao PropostaConfiguracao { get; set; }
    }
}