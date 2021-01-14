using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class AddCatalogEstados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uf",
                table: "Empresas");

            migrationBuilder.AddColumn<int>(
                name: "CatalogEstadoId",
                table: "Empresas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogEstados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogEstados", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CatalogEstadoId",
                table: "Empresas",
                column: "CatalogEstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_CatalogEstados_CatalogEstadoId",
                table: "Empresas",
                column: "CatalogEstadoId",
                principalTable: "CatalogEstados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_CatalogEstados_CatalogEstadoId",
                table: "Empresas");

            migrationBuilder.DropTable(
                name: "CatalogEstados");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_CatalogEstadoId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "CatalogEstadoId",
                table: "Empresas");

            migrationBuilder.AddColumn<string>(
                name: "Uf",
                table: "Empresas",
                nullable: true);
        }
    }
}
