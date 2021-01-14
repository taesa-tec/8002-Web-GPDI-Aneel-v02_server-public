using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class PropostasFornecedorUK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropostaFornecedor_CaptacaoId",
                table: "PropostaFornecedor");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaFornecedor_CaptacaoId_FornecedorId",
                table: "PropostaFornecedor",
                columns: new[] { "CaptacaoId", "FornecedorId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropostaFornecedor_CaptacaoId_FornecedorId",
                table: "PropostaFornecedor");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaFornecedor_CaptacaoId",
                table: "PropostaFornecedor",
                column: "CaptacaoId");
        }
    }
}
