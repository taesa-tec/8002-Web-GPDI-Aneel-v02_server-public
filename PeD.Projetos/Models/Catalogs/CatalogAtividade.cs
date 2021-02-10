using System.ComponentModel.DataAnnotations;

namespace PeD.Projetos.Models.Catalogs
{
    public class CatalogAtividade
    {
        [Key] public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}