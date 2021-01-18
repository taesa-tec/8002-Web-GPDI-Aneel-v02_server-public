using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddNewCategoriasUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_AspNetUsers_UserId",
                table: "Uploads",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_AspNetUsers_UserId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Temas_ProjetoId",
                table: "Temas",
                column: "ProjetoId");
        }
    }
}
