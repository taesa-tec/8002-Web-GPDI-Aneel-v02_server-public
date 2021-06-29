using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class Pais : BaseEntity
    {
        public string Nome { get; set; }

        public override string ToString()
        {
            return Nome;
        }
    }
}