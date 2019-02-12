using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class CatalogTema
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public ICollection<CatalogSubTema> SubTemas { get; set; }
        public int Order {get; set;}

    }
    public class CatalogSubTema
    {
        [Key]
        public int SubTemaId { get; set; }
        public int CatalogTemaId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int Order {get; set;}
    }
}