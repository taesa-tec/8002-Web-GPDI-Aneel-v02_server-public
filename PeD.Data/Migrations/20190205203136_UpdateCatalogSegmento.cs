using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class UpdateCatalogSegmento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Segmento",
                table: "CatalogSegmentos",
                newName: "Valor");

            migrationBuilder.AlterColumn<int>(
                name: "CompartResultados",
                table: "Projetos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AvaliacaoInicial",
                table: "Projetos",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "CatalogSegmentos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "CatalogSegmentos");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "CatalogSegmentos",
                newName: "Segmento");

            migrationBuilder.AlterColumn<string>(
                name: "CompartResultados",
                table: "Projetos",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvaliacaoInicial",
                table: "Projetos",
                nullable: true,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
