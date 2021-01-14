using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class AddFotoPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FotoPerfil",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "AspNetUsers");
        }
    }
}
