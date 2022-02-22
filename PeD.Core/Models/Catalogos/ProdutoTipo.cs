using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace PeD.Core.Models.Catalogos
{
    [Table("ProdutoTipos")]
    public class ProdutoTipo
    {
        public ProdutoTipo()
        {
        }

        public ProdutoTipo(string id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        [Key] public string Id { get; set; }
        public string Nome { get; set; }
    }

    public class ProdutoTipoValidator : AbstractValidator<ProdutoTipo>
    {
        public ProdutoTipoValidator()
        {
            RuleFor(r => r.Nome).NotEmpty();
        }
    }
}