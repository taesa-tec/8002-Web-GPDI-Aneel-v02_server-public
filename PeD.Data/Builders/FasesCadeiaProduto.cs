using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class FasesCadeiaProduto
    {
        private static List<FaseCadeiaProduto> _fases;

        private static List<FaseCadeiaProduto> GetFases()
        {
            if (_fases != null)
            {
                return _fases;
            }

            var fases = new List<FaseCadeiaProduto>
            {
                new FaseCadeiaProduto
                {
                    Id = "PB",
                    Nome = "Pesquisa Básica Dirigida",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado {Valor = "", Nome = "Novo material"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Nova estrutura"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Modelo"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Algoritmo"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Id = "PA",
                    Nome = "Pesquisa Aplicada",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado {Valor = "", Nome = "metodologia ou técnica"},
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Projeto demonstrativo de novos equipamentos"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Modelos digitais"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Modelos de funções ou de processos"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Id = "DE",
                    Nome = "Desenvolvimento Experimental",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Protótipo de material para demonstração e testes"},
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Protótipo de dispositivo para demonstração e testes"},
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Protótipo de equipamento para demonstração e testes"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Implantação de projeto piloto"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Serviços (novos ou aperfeiçoados)"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Software baseado em pesquisa aplicada"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Id = "CS",
                    Nome = "Cabeça de série",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Aperfeiçoamento de protótipo obtido em projeto anterior"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Id = "LP",
                    Nome = "Lote Pioneiro",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado {Valor = "", Nome = "Primeira fabricação de produto"},
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Reprodução de licenças para ensaios de validação"},
                        new FaseTipoDetalhado
                        {
                            Valor = "",
                            Nome =
                                "Análise de custos e refino do projeto, com vistas à produção industrial e/ou à comercialização"
                        }
                    }
                },
                new FaseCadeiaProduto
                {
                    Id = "IM",
                    Nome = "Inserção no Mercado",
                    TiposDetalhados = new List<FaseTipoDetalhado>
                    {
                        new FaseTipoDetalhado {Valor = "", Nome = "Estudos mercadológicos"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Material de divulgação"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Registro de patentes"},
                        new FaseTipoDetalhado
                        {
                            Valor = "",
                            Nome = "Contratação de empresa de transferência de tecnologia e serviços jurídicos"
                        },
                        new FaseTipoDetalhado
                            {Valor = "", Nome = "Aprimoramentos e melhorias incrementais nos produtos"},
                        new FaseTipoDetalhado {Valor = "", Nome = "Software ou serviços"}
                    }
                }
            };

            var ida = 1;
            fases.ForEach(f =>
            {
                f.TiposDetalhados.ForEach(t =>
                {
                    t.Id = ida++;
                    t.FaseCadeiaProdutoId = f.Id;
                });
            });
            _fases = fases;
            return fases;
        }

        public static EntityTypeBuilder<FaseCadeiaProduto> Config(this EntityTypeBuilder<FaseCadeiaProduto> builder)
        {
            return builder.Seed().ToTable("FasesCadeiaProduto");
        }

        public static EntityTypeBuilder<FaseCadeiaProduto> Seed(this EntityTypeBuilder<FaseCadeiaProduto> builder)
        {
            var fases = GetFases().Select(f => new FaseCadeiaProduto()
            {
                Id = f.Id,
                Nome = f.Nome,
            });
            builder.HasData(fases);
            return builder;
        }

        public static EntityTypeBuilder<FaseTipoDetalhado> Config(this EntityTypeBuilder<FaseTipoDetalhado> builder)
        {
            builder.Property(f => f.FaseCadeiaProdutoId).IsRequired();
            return builder.Seed();
        }

        public static EntityTypeBuilder<FaseTipoDetalhado> Seed(this EntityTypeBuilder<FaseTipoDetalhado> builder)
        {
            var tipos = GetFases().SelectMany(f => f.TiposDetalhados);
            builder.HasData(tipos);
            return builder;
        }
    }
}