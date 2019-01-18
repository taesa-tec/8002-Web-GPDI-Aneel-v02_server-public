using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class LogProjeto
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; } 
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; } 
        public string Tela { get; set; }
        public string Acao { get; set; }
        public string StatusAnterior { get; set; }
        public string StatusNovo { get; set; }
        public DateTime Created { get; set; }
    }
    public enum Telas
    {
        Projeto
    }

    public enum Acoes
    {
        Create,Retrieve,Update,Delete
    }
}