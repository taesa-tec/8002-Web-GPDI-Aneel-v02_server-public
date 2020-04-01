using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models.Projetos
{
    public class LogProjeto : Log
    {

        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; }
    }



}