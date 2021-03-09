using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostaRelatorio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAlteracao",
                table: "Propostas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "RelatorioId",
                table: "Propostas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropostaRelatorios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DataAlteracao = table.Column<DateTime>(nullable: false),
                    Validacao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRelatorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRelatorios_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_RelatorioId",
                table: "Propostas",
                column: "RelatorioId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRelatorios_PropostaId",
                table: "PropostaRelatorios",
                column: "PropostaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propostas_PropostaRelatorios_RelatorioId",
                table: "Propostas",
                column: "RelatorioId",
                principalTable: "PropostaRelatorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_PropostaRelatorios_RelatorioId",
                table: "Propostas");

            migrationBuilder.DropTable(
                name: "PropostaRelatorios");

            migrationBuilder.DropIndex(
                name: "IX_Propostas_RelatorioId",
                table: "Propostas");

            migrationBuilder.DropColumn(
                name: "DataAlteracao",
                table: "Propostas");

            migrationBuilder.DropColumn(
                name: "RelatorioId",
                table: "Propostas");
        }
    }
}
