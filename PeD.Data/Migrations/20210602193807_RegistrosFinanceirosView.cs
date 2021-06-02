using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class RegistrosFinanceirosView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(PeD.Data.Views.RegistrosFinanceiros.Up);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(PeD.Data.Views.RegistrosFinanceiros.Down);
        }
    }
}
