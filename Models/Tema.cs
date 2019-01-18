using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Tema
    {
        [Key]
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public int CatalogTemaId { get; set; }
        public CatalogTema CatalogTema { get; set; }
        public string OutroDesc { get; set; }
        public ICollection<TemaSubTema> SubTemas { get; set; }
    }
    public class TemaSubTema
    {
        [Key]
        public int Id { get; set; }
        public int? TemaId { get; set; }
        public int CatalogSubTemaId { get; set; }
        public CatalogSubTema CatalogSubTema { get; set; }
        public string OutroDesc { get; set; }
    }
}