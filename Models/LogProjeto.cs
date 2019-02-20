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
        public ApplicationUser User { get; set; } 
        public string Tela { get; set; }
        public Acoes Acao { get; set; }
        [NotMapped]
        public string AcaoValor { get => Enum.GetName(typeof(Acoes),Acao); }
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