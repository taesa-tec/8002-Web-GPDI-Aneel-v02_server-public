using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogProjetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    StatusAnterior = table.Column<string>(nullable: true),
                    StatusNovo = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogProjetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogProjetos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjetos_ProjetoId",
                table: "UserProjetos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogProjetos_UserId",
                table: "LogProjetos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos");

            migrationBuilder.DropTable(
                name: "LogProjetos");

            migrationBuilder.DropIndex(
                name: "IX_UserProjetos_ProjetoId",
                table: "UserProjetos");
        }
    }
}
