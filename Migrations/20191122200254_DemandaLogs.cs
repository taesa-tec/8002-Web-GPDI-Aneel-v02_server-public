using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class DemandaLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DemandaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    Tela = table.Column<string>(nullable: true),
                    Acao = table.Column<int>(nullable: false),
                    StatusAnterior = table.Column<string>(nullable: true),
                    StatusNovo = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DemandaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaLogs_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandaLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandaLogs_DemandaId",
                table: "DemandaLogs",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaLogs_UserId",
                table: "DemandaLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandaLogs");
        }
    }
}
