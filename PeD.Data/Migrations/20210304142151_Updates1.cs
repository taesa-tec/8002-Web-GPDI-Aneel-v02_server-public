using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Updates1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos");

            migrationBuilder.DropColumn(
                name: "PesquisasCorrelatas",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.AddColumn<string>(
                name: "PesquisasCorrelatasExecutora",
                table: "PropostaPlanosTrabalhos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PesquisasCorrelatasPeD",
                table: "PropostaPlanosTrabalhos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PesquisasCorrelatasPeDAneel",
                table: "PropostaPlanosTrabalhos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResultadoEsperado",
                table: "PropostaEscopos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Funcao",
                table: "PropostaCoExecutores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "EtapaId",
                principalTable: "PropostaEtapas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos");

            migrationBuilder.DropColumn(
                name: "PesquisasCorrelatasExecutora",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.DropColumn(
                name: "PesquisasCorrelatasPeD",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.DropColumn(
                name: "PesquisasCorrelatasPeDAneel",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.DropColumn(
                name: "ResultadoEsperado",
                table: "PropostaEscopos");

            migrationBuilder.DropColumn(
                name: "Funcao",
                table: "PropostaCoExecutores");

            migrationBuilder.AddColumn<string>(
                name: "PesquisasCorrelatas",
                table: "PropostaPlanosTrabalhos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos",
                column: "PropostaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "EtapaId",
                principalTable: "PropostaEtapas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
