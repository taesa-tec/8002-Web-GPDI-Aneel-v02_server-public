using System;
using System.Collections.Generic;
using PeD.Core.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class ComentarioDto : PropostaNodeDto
    {
        public string AuthorId { get; set; }
        public string Author { get; set; }

        public string Mensagem { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<FileUpload> Files { get; set; }
    }
}