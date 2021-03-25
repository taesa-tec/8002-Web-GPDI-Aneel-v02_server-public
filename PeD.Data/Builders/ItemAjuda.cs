using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PeD.Data.Builders
{
    public static class ItemAjuda
    {
        private static List<string> _data = new List<string>()
        {
            "alocacao-recurso-humano",
            "alocacao-recurso-humano-etapa",
            "alocacao-recurso-humano-financiadora",
            "alocacao-recurso-material",
            "alocacao-recurso-material-empresa-financiadora",
            "alocacao-recurso-material-empresa-recebedora",
            "alocacao-recurso-material-etapa",
            "alocacao-recurso-material-quantidade",
            "cadastro-co-executor-cnpj",
            "cadastro-co-executor-funcao",
            "cadastro-co-executor-razao-social",
            "cadastro-co-executor-uf",
            "cadastro-fornecedor-ativo",
            "cadastro-fornecedor-cnpj",
            "cadastro-fornecedor-email-responsavel",
            "cadastro-fornecedor-nome",
            "cadastro-fornecedor-nome-responsavel",
            "cadastro-produto-cadeia-inovacao",
            "cadastro-produto-tipo-detalhado",
            "cadastro-produto-tipo-produto",
            "cadastro-produto-titulo",
            "cadastro-usuario-email",
            "cadastro-usuario-nome-completo",
            "cadastro-usuario-tipo",
            "captacao-consideracoes",
            "captacao-contrato",
            "captacao-data-maxima",
            "captacao-data-maxima-ext",
            "captacao-data-proposta",
            "captacao-equipe",
            "captacao-observacoes",
            "contrato-titulo",
            "demanda-status",
            "demanda-superior-direto",
            "meu-cadastro-email",
            "meu-cadastro-nome-completo",
            "proposta-config-consideracoes",
            "proposta-config-data-maxima",
            "proposta-config-fornecedores",
            "recurso-humano-brasileiro",
            "recurso-humano-cpf",
            "recurso-humano-curriculo-lattes",
            "recurso-humano-empresa",
            "recurso-humano-funcao",
            "recurso-humano-nome-completo",
            "recurso-humano-titulacao",
            "recurso-humano-valor-hora",
            "recurso-material-categoria-contabil",
            "recurso-material-nome",
            "recurso-material-valor-unitario",
            "risco-classificacao",
            "risco-item",
            "risco-probabilidade"

        };

        public static void Config(this EntityTypeBuilder<Core.Models.Sistema.ItemAjuda> builder)
        {
            builder.HasIndex(i => i.Codigo).IsUnique();
            builder.HasData(_data.OrderBy(i => i).Select((c, i) => new Core.Models.Sistema.ItemAjuda()
            {
                Id = i + 1,
                Codigo = c,
                Nome = c,
                Descricao = c
            }).OrderBy(i=>i.Id));
        }
    }
}