using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using APIGestor.Attributes;
using APIGestor.Models.Catalogs;

namespace APIGestor.Models.Projetos {
    public class Tema {
        [Key]
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        [Logger("Tema", "CatalogTema.Nome")]
        public int CatalogTemaId { get; set; }
        public CatalogTema CatalogTema { get; set; }
        [Logger("Tema (Outros)")]
        public string OutroDesc { get; set; }
        [Logger("SubTemas")]
        public List<TemaSubTema> SubTemas { get; set; }
        public List<Upload> Uploads { get; set; }
    }
    public class TemaSubTema {
        [Key]
        public int Id { get; set; }
        public int? TemaId { get; set; }
        [Logger("SubTema", "CatalogSubTema.Nome")]
        public int CatalogSubTemaId { get; set; }
        public CatalogSubTema CatalogSubTema { get; set; }
        [Logger("SubTema (Outros)")]
        public string OutroDesc { get; set; }
    }
}