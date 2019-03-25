using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddCamposRefpGestao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogAtividadeId",
                table: "RegistrosFinanceiros",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_CatalogAtividadeId",
                table: "RegistrosFinanceiros",
                column: "CatalogAtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros",
                column: "CatalogCategoriaContabilGestaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosFinanceiros_CatalogAtividade_CatalogAtividadeId",
                table: "RegistrosFinanceiros",
                column: "CatalogAtividadeId",
                principalTable: "CatalogAtividade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosFinanceiros_CatalogCategoriaContabilGestao_CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros",
                column: "CatalogCategoriaContabilGestaoId",
                principalTable: "CatalogCategoriaContabilGestao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosFinanceiros_CatalogAtividade_CatalogAtividadeId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosFinanceiros_CatalogCategoriaContabilGestao_CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosFinanceiros_CatalogAtividadeId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosFinanceiros_CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropColumn(
                name: "CatalogAtividadeId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropColumn(
                name: "CatalogCategoriaContabilGestaoId",
                table: "RegistrosFinanceiros");
        }
    }
}
