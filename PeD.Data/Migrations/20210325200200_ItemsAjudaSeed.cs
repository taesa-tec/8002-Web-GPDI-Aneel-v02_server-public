using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class ItemsAjudaSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ItemAjuda",
                columns: new[] { "Id", "Codigo", "Conteudo", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "alocacao-recurso-humano", null, "alocacao-recurso-humano", "alocacao-recurso-humano" },
                    { 29, "captacao-data-proposta", null, "captacao-data-proposta", "captacao-data-proposta" },
                    { 30, "captacao-equipe", null, "captacao-equipe", "captacao-equipe" },
                    { 31, "captacao-observacoes", null, "captacao-observacoes", "captacao-observacoes" },
                    { 32, "contrato-titulo", null, "contrato-titulo", "contrato-titulo" },
                    { 33, "demanda-status", null, "demanda-status", "demanda-status" },
                    { 34, "demanda-superior-direto", null, "demanda-superior-direto", "demanda-superior-direto" },
                    { 35, "meu-cadastro-email", null, "meu-cadastro-email", "meu-cadastro-email" },
                    { 36, "meu-cadastro-nome-completo", null, "meu-cadastro-nome-completo", "meu-cadastro-nome-completo" },
                    { 37, "proposta-config-consideracoes", null, "proposta-config-consideracoes", "proposta-config-consideracoes" },
                    { 38, "proposta-config-data-maxima", null, "proposta-config-data-maxima", "proposta-config-data-maxima" },
                    { 39, "proposta-config-fornecedores", null, "proposta-config-fornecedores", "proposta-config-fornecedores" },
                    { 40, "recurso-humano-brasileiro", null, "recurso-humano-brasileiro", "recurso-humano-brasileiro" },
                    { 41, "recurso-humano-cpf", null, "recurso-humano-cpf", "recurso-humano-cpf" },
                    { 42, "recurso-humano-curriculo-lattes", null, "recurso-humano-curriculo-lattes", "recurso-humano-curriculo-lattes" },
                    { 43, "recurso-humano-empresa", null, "recurso-humano-empresa", "recurso-humano-empresa" },
                    { 44, "recurso-humano-funcao", null, "recurso-humano-funcao", "recurso-humano-funcao" },
                    { 45, "recurso-humano-nome-completo", null, "recurso-humano-nome-completo", "recurso-humano-nome-completo" },
                    { 46, "recurso-humano-titulacao", null, "recurso-humano-titulacao", "recurso-humano-titulacao" },
                    { 47, "recurso-humano-valor-hora", null, "recurso-humano-valor-hora", "recurso-humano-valor-hora" },
                    { 48, "recurso-material-categoria-contabil", null, "recurso-material-categoria-contabil", "recurso-material-categoria-contabil" },
                    { 49, "recurso-material-nome", null, "recurso-material-nome", "recurso-material-nome" },
                    { 50, "recurso-material-valor-unitario", null, "recurso-material-valor-unitario", "recurso-material-valor-unitario" },
                    { 51, "risco-classificacao", null, "risco-classificacao", "risco-classificacao" },
                    { 28, "captacao-data-maxima-ext", null, "captacao-data-maxima-ext", "captacao-data-maxima-ext" },
                    { 52, "risco-item", null, "risco-item", "risco-item" },
                    { 27, "captacao-data-maxima", null, "captacao-data-maxima", "captacao-data-maxima" },
                    { 25, "captacao-consideracoes", null, "captacao-consideracoes", "captacao-consideracoes" },
                    { 2, "alocacao-recurso-humano-etapa", null, "alocacao-recurso-humano-etapa", "alocacao-recurso-humano-etapa" },
                    { 3, "alocacao-recurso-humano-financiadora", null, "alocacao-recurso-humano-financiadora", "alocacao-recurso-humano-financiadora" },
                    { 4, "alocacao-recurso-material", null, "alocacao-recurso-material", "alocacao-recurso-material" },
                    { 5, "alocacao-recurso-material-empresa-financiadora", null, "alocacao-recurso-material-empresa-financiadora", "alocacao-recurso-material-empresa-financiadora" },
                    { 6, "alocacao-recurso-material-empresa-recebedora", null, "alocacao-recurso-material-empresa-recebedora", "alocacao-recurso-material-empresa-recebedora" },
                    { 7, "alocacao-recurso-material-etapa", null, "alocacao-recurso-material-etapa", "alocacao-recurso-material-etapa" },
                    { 8, "alocacao-recurso-material-quantidade", null, "alocacao-recurso-material-quantidade", "alocacao-recurso-material-quantidade" },
                    { 9, "cadastro-co-executor-cnpj", null, "cadastro-co-executor-cnpj", "cadastro-co-executor-cnpj" },
                    { 10, "cadastro-co-executor-funcao", null, "cadastro-co-executor-funcao", "cadastro-co-executor-funcao" },
                    { 11, "cadastro-co-executor-razao-social", null, "cadastro-co-executor-razao-social", "cadastro-co-executor-razao-social" },
                    { 12, "cadastro-co-executor-uf", null, "cadastro-co-executor-uf", "cadastro-co-executor-uf" },
                    { 13, "cadastro-fornecedor-ativo", null, "cadastro-fornecedor-ativo", "cadastro-fornecedor-ativo" },
                    { 14, "cadastro-fornecedor-cnpj", null, "cadastro-fornecedor-cnpj", "cadastro-fornecedor-cnpj" },
                    { 15, "cadastro-fornecedor-email-responsavel", null, "cadastro-fornecedor-email-responsavel", "cadastro-fornecedor-email-responsavel" },
                    { 16, "cadastro-fornecedor-nome", null, "cadastro-fornecedor-nome", "cadastro-fornecedor-nome" },
                    { 17, "cadastro-fornecedor-nome-responsavel", null, "cadastro-fornecedor-nome-responsavel", "cadastro-fornecedor-nome-responsavel" },
                    { 18, "cadastro-produto-cadeia-inovacao", null, "cadastro-produto-cadeia-inovacao", "cadastro-produto-cadeia-inovacao" },
                    { 19, "cadastro-produto-tipo-detalhado", null, "cadastro-produto-tipo-detalhado", "cadastro-produto-tipo-detalhado" },
                    { 20, "cadastro-produto-tipo-produto", null, "cadastro-produto-tipo-produto", "cadastro-produto-tipo-produto" },
                    { 21, "cadastro-produto-titulo", null, "cadastro-produto-titulo", "cadastro-produto-titulo" },
                    { 22, "cadastro-usuario-email", null, "cadastro-usuario-email", "cadastro-usuario-email" },
                    { 23, "cadastro-usuario-nome-completo", null, "cadastro-usuario-nome-completo", "cadastro-usuario-nome-completo" },
                    { 24, "cadastro-usuario-tipo", null, "cadastro-usuario-tipo", "cadastro-usuario-tipo" },
                    { 26, "captacao-contrato", null, "captacao-contrato", "captacao-contrato" },
                    { 53, "risco-probabilidade", null, "risco-probabilidade", "risco-probabilidade" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ItemAjuda",
                keyColumn: "Id",
                keyValue: 53);
        }
    }
}
