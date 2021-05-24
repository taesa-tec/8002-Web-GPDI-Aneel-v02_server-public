namespace PeD.Core.Models.Projetos
{
    public class ProjetoArquivo
    {
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }
        public int ArquivoId { get; set; }
        public FileUpload Arquivo { get; set; }
    }
}