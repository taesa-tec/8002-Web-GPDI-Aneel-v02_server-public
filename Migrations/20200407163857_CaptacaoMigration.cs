using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class CaptacaoMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Captacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    DemandaId = table.Column<int>(nullable: false),
                    CriadorId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EnvioCaptacao = table.Column<DateTime>(nullable: true),
                    Termino = table.Column<DateTime>(nullable: true),
                    Cancelamento = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Observacoes = table.Column<string>(nullable: true),
                    Consideracoes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captacoes_AspNetUsers_CriadorId",
                        column: x => x.CriadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Captacoes_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoArquivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AcessoFornecedor = table.Column<bool>(nullable: false),
                    CaptacaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoArquivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaptacaoArquivos_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoArquivos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoContratos",
                columns: table => new
                {
                    CaptacaoId = table.Column<int>(nullable: false),
                    ContratoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoContratos", x => new {x.CaptacaoId, x.ContratoId});
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_CaptacaoSugestoesFornecedores", x => new {x.FornecedorId, x.CaptacaoId});
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
                name: "CaptacoesFornecedores",
                columns: table => new
                {
                    CaptacaoId = table.Column<int>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacoesFornecedores", x => new {x.FornecedorId, x.CaptacaoId});
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoArquivos_CaptacaoId",
                table: "CaptacaoArquivos",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoArquivos_UserId",
                table: "CaptacaoArquivos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoContratos_ContratoId",
                table: "CaptacaoContratos",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSugestoesFornecedores_CaptacaoId",
                table: "CaptacaoSugestoesFornecedores",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_CriadorId",
                table: "Captacoes",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_DemandaId",
                table: "Captacoes",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacoesFornecedores_CaptacaoId",
                table: "CaptacoesFornecedores",
                column: "CaptacaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaptacaoArquivos");

            migrationBuilder.DropTable(
                name: "CaptacaoContratos");

            migrationBuilder.DropTable(
                name: "CaptacaoSugestoesFornecedores");

            migrationBuilder.DropTable(
                name: "CaptacoesFornecedores");

            migrationBuilder.DropTable(
                name: "Captacoes");
        }
    }
}