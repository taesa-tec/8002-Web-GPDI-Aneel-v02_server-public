using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class NewChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorHora",
                table: "AlocacoesRh");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicio",
                table: "Projetos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "Projetos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "DataInicio",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Projetos");

            migrationBuilder.AddColumn<decimal>(
                name: "ValorHora",
                table: "AlocacoesRh",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
