using System;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoArquivoDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool AcessoFornecedor { get; set; }
        public int CaptacaoId { get; set; }
    }
}