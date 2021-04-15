using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostaResponsavel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponsavelId",
                table: "Propostas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_ResponsavelId",
                table: "Propostas",
                column: "ResponsavelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propostas_AspNetUsers_ResponsavelId",
                table: "Propostas",
                column: "ResponsavelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.Sql(@"
            UPDATE P 
            SET P.ResponsavelId = E.ResponsavelId
            FROM Propostas AS P
            INNER JOIN Empresas AS E ON E.Id = P.FornecedorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_AspNetUsers_ResponsavelId",
                table: "Propostas");

            migrationBuilder.DropIndex(
                name: "IX_Propostas_ResponsavelId",
                table: "Propostas");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Propostas");
        }
    }
}
