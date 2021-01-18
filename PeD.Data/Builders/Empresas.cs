using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models;

namespace PeD.Data.Builders
{
    public static class Empresas
    {
        public static EntityTypeBuilder<Empresa> Config(this EntityTypeBuilder<Empresa> builder)
        {
            return builder.Seed();
        }

        public static EntityTypeBuilder<Empresa> Seed(this EntityTypeBuilder<Empresa> builder)
        {
            builder.HasData(
                new Empresa
                {
                    Nome = "TAESA",
                    Valor = "07130",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "ATE",
                    Valor = "04906",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "ATE II",
                    Valor = "05012",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "ATE III",
                    Valor = "05455",
                    Cnpj = "07.002.685/0001-54"
                },
                new Empresa
                {
                    Nome = "ETEO",
                    Valor = "	0414",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "GTESA",
                    Valor = "03624",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "MARIANA",
                    Valor = "08837",
                    Cnpj = "19.486.977/0001-99"
                },
                new Empresa
                {
                    Nome = "MUNIRAH",
                    Valor = "04757",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "NOVATRANS",
                    Valor = "02609",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "NTE",
                    Valor = "03619",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "PATESA",
                    Valor = "03943",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "São Gotardo",
                    Valor = "08193",
                    Cnpj = "15.867.360/0001-62"
                },
                new Empresa
                {
                    Nome = "STE",
                    Valor = "03944",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "TSN",
                    Valor = "02607",
                    Cnpj = "07.859.971/0001-30"
                },
                new Empresa
                {
                    Nome = "ETAU",
                    Valor = "03942",
                    Cnpj = "05.063.249/0001-60"
                },
                new Empresa
                {
                    Nome = "BRASNORTE",
                    Valor = "06625",
                    Cnpj = "09.274.998/0001-97"
                },
                new Empresa
                {
                    Nome = "MIRACEMA",
                    Valor = "10731",
                    Cnpj = "24.944.194/0001-41"
                },
                new Empresa
                {
                    Nome = "JANAÚBA",
                    Valor = "11114",
                    Cnpj = "26.617.923/0001-80"
                },
                new Empresa
                {
                    Nome = "AIMORÉS",
                    Valor = "11105",
                    Cnpj = "26.707.830/0001-47"
                },
                new Empresa
                {
                    Nome = "PARAGUAÇÚ",
                    Valor = "11104",
                    Cnpj = "26.712.591/0001-13"
                },
                new Empresa
                {
                    Nome = "ERB 1",
                    Valor = "00000",
                    Cnpj = "28.052.123/0001-95"
                });
            return builder;
        }
    }
}