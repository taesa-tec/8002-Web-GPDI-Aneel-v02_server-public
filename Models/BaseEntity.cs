using System.ComponentModel.DataAnnotations;

namespace APIGestor.Models
{
    public abstract class BaseEntity
    {
        [Key] public int Id { get; set; }
    }
}