using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class Segmentos
    {
        public static EntityTypeBuilder<Segmento> Config(this EntityTypeBuilder<Segmento> builder)
        {
            return builder.Seed();
        }

        public static EntityTypeBuilder<Segmento> Seed(this EntityTypeBuilder<Segmento> builder)
        {
            builder.HasData(
                new Segmento {Nome = "Geração", Valor = "G"},
                new Segmento {Nome = "Transmissão", Valor = "T"},
                new Segmento {Nome = "Distribuição", Valor = "D"},
                new Segmento {Nome = "Comercialização", Valor = "C"});
            return builder;
        }
    }
}