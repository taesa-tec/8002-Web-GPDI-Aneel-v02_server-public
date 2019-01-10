

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class Empresa
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        private string _nomeFantasia;
        public string NomeFantasia
        {
            get => _nomeFantasia;
            set => _nomeFantasia = value?.Trim();
        }

        public string RazaoSocial { get; set; }
        public string Uf { get; set; }
    }
}