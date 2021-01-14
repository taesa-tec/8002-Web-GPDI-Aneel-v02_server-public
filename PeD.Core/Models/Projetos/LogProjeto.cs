using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public class LogProjeto : Log
    {

        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; }
    }



}