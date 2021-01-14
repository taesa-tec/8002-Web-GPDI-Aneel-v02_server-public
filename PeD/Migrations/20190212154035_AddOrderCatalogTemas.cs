using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class AddOrderCatalogTemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CatalogTema",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "CatalogSubTemas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "CatalogTema");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "CatalogSubTemas");
        }
    }
}
