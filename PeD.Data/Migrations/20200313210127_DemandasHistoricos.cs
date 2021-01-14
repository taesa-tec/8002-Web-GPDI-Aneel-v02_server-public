using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class DemandasHistoricos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Html",
                table: "DemandaFormValues",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DemandaFormHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    FormValuesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFormHistoricos_DemandaFormValues_FormValuesId",
                        column: x => x.FormValuesId,
                        principalTable: "DemandaFormValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormHistoricos_FormValuesId",
                table: "DemandaFormHistoricos",
                column: "FormValuesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandaFormHistoricos");

            migrationBuilder.DropColumn(
                name: "Html",
                table: "DemandaFormValues");
        }
    }
}
