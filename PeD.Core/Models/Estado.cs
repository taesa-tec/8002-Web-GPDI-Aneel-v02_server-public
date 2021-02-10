using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Models
{
    public class Estado : BaseEntity
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