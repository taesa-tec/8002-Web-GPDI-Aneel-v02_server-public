using System.ComponentModel.DataAnnotations;

namespace APIGestor.Models.Catalogs
{
    public class CatalogStatus
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public string Status { get; set; }
    }

    }