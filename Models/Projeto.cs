using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Projeto
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
        public string TituloDesc { get; set; }
        public string Numero { get;set;}
        private int _empresaProponente;
        public int EmpresaProponente
        {
            get => _empresaProponente;
            set => _empresaProponente = value;
        }

        public string Status { get; set; }
        public int SegmentoId { get; set; }
        public string AvaliacaoInicial { get; set; }
        public string CompartResultados { get; set; }
        public string Motivacao { get; set; }
        public string Originalidade { get; set; }
        public string Aplicabilidade { get; set; }
        public string Relevancia { get; set; }
        public string Razoabilidade { get; set; }
        public string Pesquisas { get; set; }

        public List<Produto> Produtos { get; set; }

    }
}