using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models;

namespace PeD.Projetos.Models.Projetos
{
    public class LogProjeto : Log
    {

        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; }
    }



}