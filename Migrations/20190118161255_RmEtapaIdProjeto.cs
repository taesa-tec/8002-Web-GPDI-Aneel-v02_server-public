using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class RmEtapaIdProjeto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Temas_TemaId1",
                table: "Projetos");

            migrationBuilder.DropIndex(
                name: "IX_Projetos_TemaId1",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "TemaId",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "TemaId1",
                table: "Projetos");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas");

            migrationBuilder.DropIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas");

            migrationBuilder.AddColumn<int>(
                name: "TemaId",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemaId1",
                table: "Projetos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_TemaId1",
                table: "Projetos",
                column: "TemaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Temas_TemaId1",
                table: "Projetos",
                column: "TemaId1",
                principalTable: "Temas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
