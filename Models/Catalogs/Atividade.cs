using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class CatalogCategoriaContabilGestao
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public ICollection<CatalogAtividade> Atividades { get; set; }

    }
    public class CatalogAtividade
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}