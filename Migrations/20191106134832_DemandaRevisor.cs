using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class DemandaRevisor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RevisorId",
                table: "Demandas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_RevisorId",
                table: "Demandas",
                column: "RevisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_AspNetUsers_RevisorId",
                table: "Demandas",
                column: "RevisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_AspNetUsers_RevisorId",
                table: "Demandas");

            migrationBuilder.DropIndex(
                name: "IX_Demandas_RevisorId",
                table: "Demandas");

            migrationBuilder.DropColumn(
                name: "RevisorId",
                table: "Demandas");
        }
    }
}
