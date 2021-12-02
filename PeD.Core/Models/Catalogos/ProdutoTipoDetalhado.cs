using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class FaseTipoDetalhado : BaseEntity
    {
        [Required] public string FaseCadeiaProdutoId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}