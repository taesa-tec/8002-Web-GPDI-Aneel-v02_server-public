using TaesaCore.Models;

namespace PeD.Core.ApiModels.Catalogos
{
    public class SegmentoDto : BaseEntity
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