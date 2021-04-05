using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PeD.Core.Models.Propostas;
using Serilog;

namespace PeD.Services.Captacoes
{
    public class ContratoService
    {
        public struct Shortcode
        {
            public string Code { get; set; }
            public string Label { get; set; }
            public Func<Proposta, string> Replacer { get; set; }
        }

        public static List<Shortcode> Shortcodes =
            new List<Shortcode>()
            {
                new Shortcode()
                {
                    Label = "Fornecedor Nome",
                    Code = "Fornecedor.Nome",
                    Replacer = p => p.Fornecedor.Nome
                },
                new Shortcode()
                {
                    Label = "Fornecedor CNPJ",
                    Code = "Fornecedor.CNPJ",
                    Replacer = p =>
                        Regex.Replace(p.Fornecedor.Cnpj, @"^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$", "$1.$2.$3/$4-$5")
                },
                new Shortcode()
                {
                    Label = "Fase da cadeia de inovação",
                    Code = "Projeto.FaseCadeia",
                    Replacer = p =>
                    {
                        if (p.Produtos != null)
                        {
                            var produto =
                                p.Produtos.FirstOrDefault(pd => pd.Classificacao == ProdutoClassificacao.Final);

                            return produto?.FaseCadeia?.Nome ?? "";
                        }

                        return "";
                    }
                },
                new Shortcode()
                {
                    Label = "Prazo do projeto",
                    Code = "Projeto.Prazo",
                    Replacer = p => p.Duracao.ToString()
                },
                new Shortcode()
                {
                    Label = "Tema do projeto",
                    Code = "Projeto.Tema",
                    Replacer = p => p.Captacao.Tema?.Nome ?? p.Captacao.TemaOutro
                },
                new Shortcode()
                {
                    Label = "Titulo do projeto",
                    Code = "Projeto.Titulo",
                    Replacer = p => p.Captacao.Titulo
                },
                new Shortcode()
                {
                    Label = "Valor do projeto",
                    Code = "Projeto.Valor",
                    Replacer = p =>
                    {
                        try
                        {
                            return p.Etapas.Sum(e =>
                                e.RecursosHumanosAlocacoes.Sum(r => r.Valor) +
                                e.RecursosMateriaisAlocacoes.Sum(r => r.Valor)).ToString("C");
                        }
                        catch (Exception e)
                        {
                            return 0.ToString("C");
                        }
                    }
                },
            };

        public static List<List<string>> GetShortcodesDescriptions()
        {
            return Shortcodes.Select(src => new List<string>() {src.Code, src.Label}).ToList();
        }

        public static string ReplaceShortcodes(string text, Proposta proposta)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            foreach (var shortcode in Shortcodes)
            {
                var placeholder = @$"{{{shortcode.Code}}}";
                var value = "NÃO ENCONTRADO";
                try
                {
                    value = shortcode.Replacer(proposta);
                }
                catch (Exception e)
                {
                    Log.Error("Erro shortcode {Shortcode}: {Message}\n {Trace}", placeholder, e.Message, e.StackTrace);
                }
                finally
                {
                    text = text.Replace(placeholder, value);
                }
            }

            return text;
        }
    }
}