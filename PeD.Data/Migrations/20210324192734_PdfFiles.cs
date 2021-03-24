using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PdfFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "PropostaRelatorios",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "PropostaContratos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRelatorios_FileId",
                table: "PropostaRelatorios",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_FileId",
                table: "PropostaContratos",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaContratos_Files_FileId",
                table: "PropostaContratos",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRelatorios_Files_FileId",
                table: "PropostaRelatorios",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaContratos_Files_FileId",
                table: "PropostaContratos");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRelatorios_Files_FileId",
                table: "PropostaRelatorios");

            migrationBuilder.DropIndex(
                name: "IX_PropostaRelatorios_FileId",
                table: "PropostaRelatorios");

            migrationBuilder.DropIndex(
                name: "IX_PropostaContratos_FileId",
                table: "PropostaContratos");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "PropostaRelatorios");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "PropostaContratos");
        }
    }
}
