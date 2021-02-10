using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class CategoriaContabil : BaseEntity
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
        public List<CategoriaContabilAtividade> Atividades { get; set; }
    }
}