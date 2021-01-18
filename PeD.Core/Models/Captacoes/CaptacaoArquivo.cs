using System;
using Newtonsoft.Json;

namespace PeD.Core.Models.Captacoes
{
    public class CaptacaoArquivo : IFileUpload
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long Size { get; set; }
        [JsonIgnore] public string Path { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool AcessoFornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }
    }
}