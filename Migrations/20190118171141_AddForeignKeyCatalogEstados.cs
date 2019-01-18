using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddForeignKeyCatalogEstados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LogProjetos_ProjetoId",
                table: "LogProjetos",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos");

            migrationBuilder.DropIndex(
                name: "IX_LogProjetos_ProjetoId",
                table: "LogProjetos");
        }
    }
}
