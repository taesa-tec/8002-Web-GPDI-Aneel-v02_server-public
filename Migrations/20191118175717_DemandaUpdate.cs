using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class DemandaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CaptacaoDate",
                table: "Demandas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Revisao",
                table: "DemandaFormValues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "DemandaFormFiles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaptacaoDate",
                table: "Demandas");

            migrationBuilder.DropColumn(
                name: "Revisao",
                table: "DemandaFormValues");

            migrationBuilder.AlterColumn<int>(
                name: "FileId",
                table: "DemandaFormFiles",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
