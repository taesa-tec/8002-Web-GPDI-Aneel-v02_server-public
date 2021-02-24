using Microsoft.EntityFrameworkCore.Migrations;
using PeD.Data.Views;

namespace PeD.Data.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Captacoes.Up);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(Captacoes.Down);
        }
    }
}