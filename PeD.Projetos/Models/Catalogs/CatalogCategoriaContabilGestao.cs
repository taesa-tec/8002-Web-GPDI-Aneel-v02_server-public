using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PeD.Projetos.Models.Catalogs
{
    public class CatalogCategoriaContabilGestao
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public ICollection<PeD.Projetos.Models.Catalogs.CatalogAtividade> Atividades { get; set; }

    }
}