using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostaSelecao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArquivoComprobatorioId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAlvo",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropostaSelecionadaId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioRefinamentoId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoComprobatorioId",
                table: "Captacoes",
                column: "ArquivoComprobatorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_PropostaSelecionadaId",
                table: "Captacoes",
                column: "PropostaSelecionadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioRefinamentoId",
                table: "Captacoes",
                column: "UsuarioRefinamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoComprobatorioId",
                table: "Captacoes",
                column: "ArquivoComprobatorioId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Propostas_PropostaSelecionadaId",
                table: "Captacoes",
                column: "PropostaSelecionadaId",
                principalTable: "Propostas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioRefinamentoId",
                table: "Captacoes",
                column: "UsuarioRefinamentoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Files_ArquivoComprobatorioId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Propostas_PropostaSelecionadaId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioRefinamentoId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_ArquivoComprobatorioId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_PropostaSelecionadaId",
                table: "Captacoes");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_UsuarioRefinamentoId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "ArquivoComprobatorioId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "DataAlvo",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "PropostaSelecionadaId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioRefinamentoId",
                table: "Captacoes");
        }
    }
}
