using System.ComponentModel.DataAnnotations;

namespace PeD.Projetos.Models.Catalogs
{
    public class Pais
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
    }
}