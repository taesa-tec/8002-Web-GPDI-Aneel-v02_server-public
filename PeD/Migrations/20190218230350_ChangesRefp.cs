using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class ChangesRefp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs");

            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Categoria",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorUnitario",
                table: "RegistrosFinanceiros",
                type: "decimal(18, 2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)");

            migrationBuilder.AlterColumn<int>(
                name: "TipoDocumento",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QtdItens",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "QtdHrs",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Mes",
                table: "RegistrosFinanceiros",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ItemNacional",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "EquiparLabNovo",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "EquiparLabExistente",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<int>(
                name: "RegistroFinanceiroId",
                table: "RegistroObs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FotoPerfilId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FotoPerfil",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FotoPerfil", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_RegistroFinanceiroId",
                table: "Uploads",
                column: "RegistroFinanceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FotoPerfilId",
                table: "AspNetUsers",
                column: "FotoPerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FotoPerfil_FotoPerfilId",
                table: "AspNetUsers",
                column: "FotoPerfilId",
                principalTable: "FotoPerfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "Uploads",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FotoPerfil_FotoPerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "Uploads");

            migrationBuilder.DropTable(
                name: "FotoPerfil");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_RegistroFinanceiroId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FotoPerfilId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FotoPerfilId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Categoria",
                table: "Uploads",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ValorUnitario",
                table: "RegistrosFinanceiros",
                type: "decimal(18, 2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TipoDocumento",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "QtdItens",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QtdHrs",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mes",
                table: "RegistrosFinanceiros",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "ItemNacional",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EquiparLabNovo",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "EquiparLabExistente",
                table: "RegistrosFinanceiros",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegistroFinanceiroId",
                table: "RegistroObs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<byte[]>(
                name: "FotoPerfil",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
