using System.ComponentModel.DataAnnotations;

namespace PeD.Projetos.Models.Catalogs
{
    public class CatalogSubTema
    {
        [Key] public int SubTemaId { get; set; }
        public int CatalogTemaId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public int Order { get; set; }
    }
}