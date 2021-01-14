using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class AddCnpjCatalogEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "CatalogEmpresas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatorioFinal_ProjetoId",
                table: "RelatorioFinal",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal");

            migrationBuilder.DropIndex(
                name: "IX_RelatorioFinal_ProjetoId",
                table: "RelatorioFinal");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "CatalogEmpresas");
        }
    }
}
