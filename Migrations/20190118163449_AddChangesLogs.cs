using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddChangesLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Acao",
                table: "LogProjetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tela",
                table: "LogProjetos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Acao",
                table: "LogProjetos");

            migrationBuilder.DropColumn(
                name: "Tela",
                table: "LogProjetos");
        }
    }
}
