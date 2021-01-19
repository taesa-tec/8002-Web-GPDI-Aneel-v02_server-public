using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PeD.Core.Models.Catalogos;

namespace PeD.Data.Builders
{
    public static class FasesCadeiaProduto
    {
        public static EntityTypeBuilder<FaseCadeiaProduto> Config(this EntityTypeBuilder<FaseCadeiaProduto> builder)
        {
            return builder.Seed().ToTable("FasesCadeiaProduto");
        }

        public static EntityTypeBuilder<FaseCadeiaProduto> Seed(this EntityTypeBuilder<FaseCadeiaProduto> builder)
        {
            var fases = new List<FaseCadeiaProduto>
            {
                new FaseCadeiaProduto
                {
                    Valor = "PB",
                    Nome = "Pesquisa Básica Dirigida",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Novo material"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Nova estrutura"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelo"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Algoritmo"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Valor = "PA",
                    Nome = "Pesquisa Aplicada",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "metodologia ou técnica"},
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Projeto demonstrativo de novos equipamentos"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelos digitais"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Modelos de funções ou de processos"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Valor = "DE",
                    Nome = "Desenvolvimento Experimental",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Protótipo de material para demonstração e testes"},
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Protótipo de dispositivo para demonstração e testes"},
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Protótipo de equipamento para demonstração e testes"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Implantação de projeto piloto"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Serviços (novos ou aperfeiçoados)"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Software baseado em pesquisa aplicada"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Valor = "CS",
                    Nome = "Cabeça de série",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Aperfeiçoamento de protótipo obtido em projeto anterior"}
                    }
                },
                new FaseCadeiaProduto
                {
                    Valor = "LP",
                    Nome = "Lote Pioneiro",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Primeira fabricação de produto"},
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Reprodução de licenças para ensaios de validação"},
                        new CatalogProdutoTipoDetalhado
                        {
                            Valor = "",
                            Nome =
                                "Análise de custos e refino do projeto, com vistas à produção industrial e/ou à comercialização"
                        }
                    }
                },
                new FaseCadeiaProduto
                {
                    Valor = "IM",
                    Nome = "Inserção no Mercado",
                    TiposDetalhados = new List<CatalogProdutoTipoDetalhado>
                    {
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Estudos mercadológicos"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Material de divulgação"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Registro de patentes"},
                        new CatalogProdutoTipoDetalhado
                        {
                            Valor = "",
                            Nome = "Contratação de empresa de transferência de tecnologia e serviços jurídicos"
                        },
                        new CatalogProdutoTipoDetalhado
                            {Valor = "", Nome = "Aprimoramentos e melhorias incrementais nos produtos"},
                        new CatalogProdutoTipoDetalhado {Valor = "", Nome = "Software ou serviços"}
                    }
                }
            };
            builder.HasData(fases);
            return builder;
        }
    }
}