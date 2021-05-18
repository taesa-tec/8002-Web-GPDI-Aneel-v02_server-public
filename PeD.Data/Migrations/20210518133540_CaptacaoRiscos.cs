using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class CaptacaoRiscos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoRiscosId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAprovacaoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoRiscosId",
                table: "Captacoes",
                column: "ArquivoRiscosId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioAprovacaoId",
                table: "Captacoes",
                column: "UsuarioAprovacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoRiscosId",
                table: "Captacoes",
                column: "ArquivoRiscosId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioAprovacaoId",
                table: "Captacoes",
                column: "UsuarioAprovacaoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Files_ArquivoRiscosId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioAprovacaoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_ArquivoRiscosId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_UsuarioAprovacaoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "ArquivoRiscosId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioAprovacaoId",
                table: "Captacoes");
        }
    }
}
