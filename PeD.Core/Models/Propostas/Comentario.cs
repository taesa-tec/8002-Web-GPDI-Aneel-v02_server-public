using System;
using System.Collections.Generic;

namespace PeD.Core.Models.Propostas
{
    public class Comentario : PropostaNode
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public string Mensagem { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ComentarioFile<T> where T : Comentario
    {
        public int ComentarioId { get; set; }
        public T Comentario { get; set; }
        public int FileId { get; set; }
        public FileUpload File { get; set; }
    }

    public class PlanoComentarioFile : ComentarioFile<PlanoComentario>
    {
    }

    public class ContratoComentarioFile : ComentarioFile<ContratoComentario>
    {
    }

    public class PlanoComentario : Comentario
    {
        public List<PlanoComentarioFile> Files { get; set; }
    }

    public class ContratoComentario : Comentario
    {
        public List<ContratoComentarioFile> Files { get; set; }
    }
}