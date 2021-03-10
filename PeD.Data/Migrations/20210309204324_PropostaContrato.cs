using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostaContrato : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaptacaoContratos");

            migrationBuilder.DropIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos");

            migrationBuilder.AddColumn<int>(
                name: "ContratoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ContratoId",
                table: "Captacoes",
                column: "ContratoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Contratos_ContratoId",
                table: "Captacoes",
                column: "ContratoId",
                principalTable: "Contratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Contratos_ContratoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_ContratoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "ContratoId",
                table: "Captacoes");

            migrationBuilder.CreateTable(
                name: "CaptacaoContratos",
                columns: table => new
                {
                    CaptacaoId = table.Column<int>(type: "int", nullable: false),
                    ContratoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoContratos", x => new { x.CaptacaoId, x.ContratoId });
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoContratos_ContratoId",
                table: "CaptacaoContratos",
                column: "ContratoId");
        }
    }
}
