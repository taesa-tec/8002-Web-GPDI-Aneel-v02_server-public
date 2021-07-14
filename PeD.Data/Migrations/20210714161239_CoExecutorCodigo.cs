using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class CoExecutorCodigo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "PropostaCoExecutores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "ProjetoCoExecutores",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UF",
                table: "Empresas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "PropostaCoExecutores");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "ProjetoCoExecutores");

            migrationBuilder.DropColumn(
                name: "UF",
                table: "Empresas");
        }
    }
}
