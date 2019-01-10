using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Produto
    {
        public DateTime Created { get; set; }
        private int _id;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private string _titulo;
        public string Titulo
        {
            get => _titulo;
            set => _titulo = value?.Trim();
        }
        public string Desc { get; set; }
   
        public int ProjetoId { get; set; }
        public Projeto Projeto { get; set; }

    }
}