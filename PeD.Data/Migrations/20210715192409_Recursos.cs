using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Recursos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoraMeses",
                table: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.CreateTable(
                name: "PropostasAlocacaoRhHorasMeses",
                columns: table => new
                {
                    AlocacaoRhId = table.Column<int>(nullable: false),
                    Mes = table.Column<int>(nullable: false),
                    Horas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostasAlocacaoRhHorasMeses", x => new { x.AlocacaoRhId, x.Mes });
                    table.ForeignKey(
                        name: "FK_PropostasAlocacaoRhHorasMeses_PropostaRecursosHumanosAlocacao_AlocacaoRhId",
                        column: x => x.AlocacaoRhId,
                        principalTable: "PropostaRecursosHumanosAlocacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropostasAlocacaoRhHorasMeses");

            migrationBuilder.AddColumn<string>(
                name: "HoraMeses",
                table: "PropostaRecursosHumanosAlocacao",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
