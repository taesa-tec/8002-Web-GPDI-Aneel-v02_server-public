using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostaEtapas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Duracao",
                table: "Propostas",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "Meses",
                table: "PropostaEtapas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "PropostaEtapas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapas_ProdutoId",
                table: "PropostaEtapas",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaEtapas_PropostaProdutos_ProdutoId",
                table: "PropostaEtapas",
                column: "ProdutoId",
                principalTable: "PropostaProdutos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaEtapas_PropostaProdutos_ProdutoId",
                table: "PropostaEtapas");

            migrationBuilder.DropIndex(
                name: "IX_PropostaEtapas_ProdutoId",
                table: "PropostaEtapas");

            migrationBuilder.DropColumn(
                name: "Duracao",
                table: "Propostas");

            migrationBuilder.DropColumn(
                name: "Meses",
                table: "PropostaEtapas");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "PropostaEtapas");
        }
    }
}
