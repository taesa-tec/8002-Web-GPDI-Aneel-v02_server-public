namespace PeD.Core.Models.Propostas
{
    public class PropostaArquivo
    {
        public int PropostaId { get; set; }
        public Proposta Proposta { get; set; }
        public int ArquivoId { get; set; }
        public FileUpload Arquivo { get; set; }
    }
}