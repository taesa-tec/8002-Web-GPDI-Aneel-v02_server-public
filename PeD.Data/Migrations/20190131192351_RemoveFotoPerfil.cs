using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class RemoveFotoPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_Projetos_ProjetoId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_AspNetUsers_UserId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_ProjetoId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_RegistroFinanceiroId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_ProjetoId",
                table: "Uploads",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_RegistroFinanceiroId",
                table: "Uploads",
                column: "RegistroFinanceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_Projetos_ProjetoId",
                table: "Uploads",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "Uploads",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_AspNetUsers_UserId",
                table: "Uploads",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
