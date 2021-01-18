using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_TemaSubTemas_CatalogTema_CatalogTemaId",
                table: "TemaSubTemas");

            migrationBuilder.DropIndex(
                name: "IX_TemaSubTemas_CatalogTemaId",
                table: "TemaSubTemas");

            migrationBuilder.DropColumn(
                name: "CatalogTemaId",
                table: "TemaSubTemas");

            migrationBuilder.AlterColumn<int>(
                name: "CatalogTemaId",
                table: "CatalogSubTemas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas",
                column: "CatalogTemaId",
                principalTable: "CatalogTema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas");

            migrationBuilder.AddColumn<int>(
                name: "CatalogTemaId",
                table: "TemaSubTemas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CatalogTemaId",
                table: "CatalogSubTemas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_TemaSubTemas_CatalogTemaId",
                table: "TemaSubTemas",
                column: "CatalogTemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas",
                column: "CatalogTemaId",
                principalTable: "CatalogTema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemaSubTemas_CatalogTema_CatalogTemaId",
                table: "TemaSubTemas",
                column: "CatalogTemaId",
                principalTable: "CatalogTema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
