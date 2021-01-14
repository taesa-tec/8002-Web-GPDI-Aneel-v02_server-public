using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddEtapas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DataInicio",
                table: "Etapas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Etapas",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "EtapaProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EtapaId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtapaProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtapaProdutos_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EtapaProdutos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EtapaProdutos_EtapaId",
                table: "EtapaProdutos",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_EtapaProdutos_ProdutoId",
                table: "EtapaProdutos",
                column: "ProdutoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EtapaProdutos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataInicio",
                table: "Etapas",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataFim",
                table: "Etapas",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
