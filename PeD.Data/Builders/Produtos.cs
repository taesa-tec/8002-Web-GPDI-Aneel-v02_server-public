using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class Produtos
    {
        public static EntityTypeBuilder<ProdutoTipo> Seed(this EntityTypeBuilder<ProdutoTipo> builder)
        {
            var tipos = new[]
            {
                new ProdutoTipo("CD", "Componente ou Dispositivo"),
                new ProdutoTipo("CM", "Conceito ou Metodologia"),
                new ProdutoTipo("ME", "Máquina ou Equipamento"),
                new ProdutoTipo("MS", "Material ou Substância"),
                new ProdutoTipo("SM", "Sistema"),
                new ProdutoTipo("SW", "Software")
            };
            builder.HasData(tipos);
            return builder;
        }
    }
}