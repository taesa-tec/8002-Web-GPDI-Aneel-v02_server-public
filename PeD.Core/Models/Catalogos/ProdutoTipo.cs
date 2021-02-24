using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Catalogos
{
    [Table("ProdutoTipos")]
    public class ProdutoTipo
    {
        [Key] public string Id { get; set; }
        public string Nome { get; set; }
    }
}