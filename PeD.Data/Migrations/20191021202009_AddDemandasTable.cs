using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddDemandasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemandaFormValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DemandaId = table.Column<int>(nullable: false),
                    FormKey = table.Column<string>(nullable: true),
                    Data = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demandas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Titulo = table.Column<string>(nullable: true),
                    CriadorId = table.Column<string>(nullable: true),
                    SuperiorDiretoId = table.Column<string>(nullable: true),
                    EtapaAtual = table.Column<int>(nullable: false),
                    EtapaStatus = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demandas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demandas_AspNetUsers_CriadorId",
                        column: x => x.CriadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Demandas_AspNetUsers_SuperiorDiretoId",
                        column: x => x.SuperiorDiretoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemandaComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DemandaId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaComentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaComentarios_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandaComentarios_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemandaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DemandaId = table.Column<int>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFiles_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EtapaProdutos_ProdutoId",
                table: "EtapaProdutos",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaComentarios_DemandaId",
                table: "DemandaComentarios",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaComentarios_UserId",
                table: "DemandaComentarios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFiles_DemandaId",
                table: "DemandaFiles",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_CriadorId",
                table: "Demandas",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_SuperiorDiretoId",
                table: "Demandas",
                column: "SuperiorDiretoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EtapaProdutos_Produtos_ProdutoId",
                table: "EtapaProdutos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EtapaProdutos_Produtos_ProdutoId",
                table: "EtapaProdutos");

            migrationBuilder.DropTable(
                name: "DemandaComentarios");

            migrationBuilder.DropTable(
                name: "DemandaFiles");

            migrationBuilder.DropTable(
                name: "DemandaFormValues");

            migrationBuilder.DropTable(
                name: "Demandas");

            migrationBuilder.DropIndex(
                name: "IX_EtapaProdutos_ProdutoId",
                table: "EtapaProdutos");
        }
    }
}
