using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class DemandaUpdateStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("EtapaStatus", "Demandas", "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Status", "Demandas", "EtapaStatus");
        }
    }
}
