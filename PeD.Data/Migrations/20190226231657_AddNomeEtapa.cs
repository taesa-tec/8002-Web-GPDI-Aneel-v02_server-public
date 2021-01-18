using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddNomeEtapa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TipoDocumento",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaContabil",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Etapas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Etapas");

            migrationBuilder.AlterColumn<int>(
                name: "TipoDocumento",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaContabil",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
