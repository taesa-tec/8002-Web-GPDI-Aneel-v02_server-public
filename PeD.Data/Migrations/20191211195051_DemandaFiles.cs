using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class DemandaFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DemandaId",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Files",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DemandaId",
                table: "Files",
                column: "DemandaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Demandas_DemandaId",
                table: "Files",
                column: "DemandaId",
                principalTable: "Demandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Demandas_DemandaId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_DemandaId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DemandaId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Files");
        }
    }
}
