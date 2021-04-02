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
        private static Dictionary<string, Func<Proposta, string>> shortcodes =
            new Dictionary<string, Func<Proposta, string>>()
            {
                {
                    "Date.Today",
                    p => DateTime.Today.ToString("d")
                },
                {
                    "Fornecedor.Nome",
                    p => p.Fornecedor.Nome
                },
                {
                    "Fornecedor.CNPJ",
                    p => Regex.Replace(p.Fornecedor.Cnpj, @"^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$", "$1.$2.$3/$4-$5")
                },
                {
                    "Projeto.FaseCadeia",
                    p => p.Produtos.FirstOrDefault(pd => pd.Classificacao == ProdutoClassificacao.Final)?.FaseCadeia
                        .Nome ?? ""
                },
                {
                    "Projeto.Prazo",
                    p => p.Duracao.ToString()
                },
                {
                    "Projeto.Tema",
                    p => p.Captacao.Tema?.Nome ?? p.Captacao.TemaOutro
                },
                {
                    "Projeto.Titulo",
                    p => p.Captacao.Titulo
                },
                {
                    "Projeto.Valor",
                    p =>
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

        public static string ReplaceShortcodes(string text, Proposta proposta)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";
            foreach (var replacer in shortcodes)
            {
                var shortcode = @$"{{{replacer.Key}}}";
                var value = "NÃO ENCONTRADO";
                try
                {
                    value = replacer.Value(proposta);
                }
                catch (Exception e)
                {
                    Log.Error("Erro shortcode {Shortcode}: {Message}\n {Trace}", shortcode, e.Message, e.StackTrace);
                }
                finally
                {
                    text = text.Replace(shortcode, value);
                }
            }

            return text;
        }
    }
}