using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class Segmento : BaseEntity
    {
        private string _nome;

        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim();
        }

        public string Valor { get; set; }
    }
}