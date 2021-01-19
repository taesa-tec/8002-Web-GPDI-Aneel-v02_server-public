using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models;

namespace PeD.Data.Builders
{
    public static class Estados
    {
        public static EntityTypeBuilder<Estado> Config(this EntityTypeBuilder<Estado> builder)
        {
            return builder.Seed().ToTable("Estados");
        }

        public static EntityTypeBuilder<Estado> Seed(this EntityTypeBuilder<Estado> builder)
        {
            builder.HasData(
                new Estado {Nome = "ACRE", Valor = "AC"},
                new Estado {Nome = "ALAGOAS", Valor = "AL"},
                new Estado {Nome = "AMAPÁ", Valor = "AP"},
                new Estado {Nome = "AMAZONAS", Valor = "AM"},
                new Estado {Nome = "BAHIA", Valor = "BA"},
                new Estado {Nome = "CEARÁ", Valor = "CE"},
                new Estado {Nome = "DISTRITO FEDERAL", Valor = "DF"},
                new Estado {Nome = "ESPÍRITO SANTO", Valor = "ES"},
                new Estado {Nome = "GOIÁS", Valor = "GO"},
                new Estado {Nome = "MARANHÃO", Valor = "MA"},
                new Estado {Nome = "MATO GROSSO", Valor = "MT"},
                new Estado {Nome = "MATO GROSSO DO SUL", Valor = "MS"},
                new Estado {Nome = "MINAS GERAIS", Valor = "MG"},
                new Estado {Nome = "PARÁ", Valor = "PA"},
                new Estado {Nome = "PARAÍBA", Valor = "PB"},
                new Estado {Nome = "PARANÁ", Valor = "PR"},
                new Estado {Nome = "PERNAMBUCO", Valor = "PE"},
                new Estado {Nome = "PIAUÍ", Valor = "PI"},
                new Estado {Nome = "RIO DE JANEIRO", Valor = "RJ"},
                new Estado {Nome = "RIO GRANDE DO NORTE", Valor = "RN"},
                new Estado {Nome = "RIO GRANDE DO SUL", Valor = "RS"},
                new Estado {Nome = "RONDONIA", Valor = "RO"},
                new Estado {Nome = "RORAIMA", Valor = "RR"},
                new Estado {Nome = "SANTA CATARINA", Valor = "SC"},
                new Estado {Nome = "SÃO PAULO", Valor = "SP"},
                new Estado {Nome = "SERGIPE", Valor = "SE"},
                new Estado {Nome = "TOCANTINS", Valor = "TO"});
            return builder;
        }
    }
}