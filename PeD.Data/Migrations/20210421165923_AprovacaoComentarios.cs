using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AprovacaoComentarios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContratoAprovacao",
                table: "Propostas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanoTrabalhoAprovacao",
                table: "Propostas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContratoComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoComentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContratoComentarios_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContratoComentarios_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanoComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoComentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoComentarios_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanoComentarios_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarios_AuthorId",
                table: "ContratoComentarios",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarios_PropostaId",
                table: "ContratoComentarios",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarios_AuthorId",
                table: "PlanoComentarios",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarios_PropostaId",
                table: "PlanoComentarios",
                column: "PropostaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratoComentarios");

            migrationBuilder.DropTable(
                name: "PlanoComentarios");

            migrationBuilder.DropColumn(
                name: "ContratoAprovacao",
                table: "Propostas");

            migrationBuilder.DropColumn(
                name: "PlanoTrabalhoAprovacao",
                table: "Propostas");
        }
    }
}
