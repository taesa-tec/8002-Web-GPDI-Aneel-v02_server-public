using System;

namespace PeD.Core.Models.Propostas
{
    public class Comentario : PropostaNode
    {
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public string Mensagem { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PlanoComentario : Comentario
    {
    }

    public class ContratoComentario : Comentario
    {
    }
}