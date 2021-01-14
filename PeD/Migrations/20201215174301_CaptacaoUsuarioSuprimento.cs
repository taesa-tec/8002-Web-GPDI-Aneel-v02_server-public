using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class CaptacaoUsuarioSuprimento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioSuprimentoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioSuprimentoId",
                table: "Captacoes",
                column: "UsuarioSuprimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioSuprimentoId",
                table: "Captacoes",
                column: "UsuarioSuprimentoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioSuprimentoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_UsuarioSuprimentoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioSuprimentoId",
                table: "Captacoes");
        }
    }
}
