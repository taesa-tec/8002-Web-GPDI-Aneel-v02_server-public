using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddEtapas1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Etapas_EtapaId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_EtapaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "EtapaId",
                table: "Produtos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EtapaId",
                table: "Produtos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_EtapaId",
                table: "Produtos",
                column: "EtapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Etapas_EtapaId",
                table: "Produtos",
                column: "EtapaId",
                principalTable: "Etapas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
