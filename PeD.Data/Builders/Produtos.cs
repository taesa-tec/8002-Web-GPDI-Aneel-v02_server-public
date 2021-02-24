using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class Produtos
    {
        public static EntityTypeBuilder<ProdutoTipo> Seed(this EntityTypeBuilder<ProdutoTipo> builder)
        {
            var tipos = new ProdutoTipo[]
            {
                new ProdutoTipo() {Id = "CD", Nome = "Componente ou Dispositivo"},
                new ProdutoTipo() {Id = "CM", Nome = "Conceito ou Metodologia"},
                new ProdutoTipo() {Id = "ME", Nome = "Máquina ou Equipamento"},
                new ProdutoTipo() {Id = "MS", Nome = "Material ou Substância"},
                new ProdutoTipo() {Id = "SM", Nome = "Sistema"},
                new ProdutoTipo() {Id = "SW", Nome = "Software"},
            };
            builder.HasData(tipos);
            return builder;
        }
    }
}