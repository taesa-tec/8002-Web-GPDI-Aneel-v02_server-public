using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeD.Projetos.Models.Catalogs
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
}