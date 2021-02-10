using System.ComponentModel.DataAnnotations;
using PeD.Core.Attributes;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class FaseTipoDetalhado : BaseEntity
    {
        public int FaseCadeiaProdutoId { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
    }
}