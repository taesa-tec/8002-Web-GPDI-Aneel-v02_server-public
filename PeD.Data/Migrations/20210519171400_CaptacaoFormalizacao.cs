using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class CaptacaoFormalizacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoFormalizacaoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProjetoAprovado",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioExecucaoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoFormalizacaoId",
                table: "Captacoes",
                column: "ArquivoFormalizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioExecucaoId",
                table: "Captacoes",
                column: "UsuarioExecucaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoFormalizacaoId",
                table: "Captacoes",
                column: "ArquivoFormalizacaoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioExecucaoId",
                table: "Captacoes",
                column: "UsuarioExecucaoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Files_ArquivoFormalizacaoId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioExecucaoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_ArquivoFormalizacaoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_UsuarioExecucaoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "ArquivoFormalizacaoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "IsProjetoAprovado",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioExecucaoId",
                table: "Captacoes");
        }
    }
}
