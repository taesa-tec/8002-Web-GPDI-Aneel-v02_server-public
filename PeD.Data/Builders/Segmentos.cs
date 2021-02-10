using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class Segmentos
    {
        public static EntityTypeBuilder<Segmento> Config(this EntityTypeBuilder<Segmento> builder)
        {
            return builder.Seed().ToTable("Segmentos");
        }

        public static EntityTypeBuilder<Segmento> Seed(this EntityTypeBuilder<Segmento> builder)
        {
            builder.HasData(
                new Segmento {Id = 1, Nome = "Geração", Valor = "G"},
                new Segmento {Id = 2, Nome = "Transmissão", Valor = "T"},
                new Segmento {Id = 3, Nome = "Distribuição", Valor = "D"},
                new Segmento {Id = 4, Nome = "Comercialização", Valor = "C"});
            return builder;
        }
    }
}