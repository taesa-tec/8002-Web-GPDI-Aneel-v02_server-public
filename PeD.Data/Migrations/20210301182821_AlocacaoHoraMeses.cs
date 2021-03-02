using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AlocacaoHoraMeses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HoraMeses",
                table: "PropostaRecursosHumanosAlocacao",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Apoio à realização do CITENEL.", "AC" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 6, "Contratação de auditoria contábil e financeira para os projetos concluídos.", "CA" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 7, "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação.", "EC" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 7, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI).", "BA" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "CITENEL", "CT" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Auditoria Contábil e Financeira", "AC" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Outros", "OU" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraMeses",
                table: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação.", "EC" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 5, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 5, "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI).", "BA" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CategoriaContabilId", "Nome", "Valor" },
                values: new object[] { 6, "Apoio à realização do CITENEL.", "AC" });

            migrationBuilder.UpdateData(
                table: "CategoriaContabilAtividades",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Contratação de auditoria contábil e financeira para os projetos concluídos.", "CA" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Outros", "OU" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "CITENEL", "CT" });

            migrationBuilder.UpdateData(
                table: "CategoriasContabeis",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Nome", "Valor" },
                values: new object[] { "Auditoria Contábil e Financeira", "AC" });
        }
    }
}
