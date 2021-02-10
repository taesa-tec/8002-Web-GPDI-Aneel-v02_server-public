using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Models
{
    public class Pais : BaseEntity
    {
        private string _nome;

        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim();
        }
    }
}