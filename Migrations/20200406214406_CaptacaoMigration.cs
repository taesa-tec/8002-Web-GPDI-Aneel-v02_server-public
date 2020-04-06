using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class CaptacaoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaptacaoId",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Captacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Observacoes = table.Column<string>(nullable: true),
                    DataTermino = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoSugestoesFornecedores",
                columns: table => new
                {
                    FornecedorId = table.Column<int>(nullable: false),
                    CaptacaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoSugestoesFornecedores", x => new { x.FornecedorId, x.CaptacaoId });
                    table.ForeignKey(
                        name: "FK_CaptacaoSugestoesFornecedores_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoSugestoesFornecedores_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaConfiguracoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaptacaoId = table.Column<int>(nullable: false),
                    Consideracoes = table.Column<string>(nullable: true),
                    DataMaxima = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaConfiguracoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaConfiguracoes_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaptacoesFornecedores",
                columns: table => new
                {
                    PropostaConfiguracaoId = table.Column<int>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacoesFornecedores", x => new { x.FornecedorId, x.PropostaConfiguracaoId });
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_PropostaConfiguracoes_PropostaConfiguracaoId",
                        column: x => x.PropostaConfiguracaoId,
                        principalTable: "PropostaConfiguracoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_CaptacaoId",
                table: "Files",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSugestoesFornecedores_CaptacaoId",
                table: "CaptacaoSugestoesFornecedores",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacoesFornecedores_PropostaConfiguracaoId",
                table: "CaptacoesFornecedores",
                column: "PropostaConfiguracaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaConfiguracoes_CaptacaoId",
                table: "PropostaConfiguracoes",
                column: "CaptacaoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Captacoes_CaptacaoId",
                table: "Files",
                column: "CaptacaoId",
                principalTable: "Captacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Captacoes_CaptacaoId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "CaptacaoSugestoesFornecedores");

            migrationBuilder.DropTable(
                name: "CaptacoesFornecedores");

            migrationBuilder.DropTable(
                name: "PropostaConfiguracoes");

            migrationBuilder.DropTable(
                name: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Files_CaptacaoId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "CaptacaoId",
                table: "Files");
        }
    }
}
