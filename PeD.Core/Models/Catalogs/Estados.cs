using System.ComponentModel.DataAnnotations;

namespace PeD.Core.Models.Catalogs
{
    public class CatalogEstado
    {   
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        private string _nome;
        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim();
        }
        public string Valor { get; set;}
    }
}