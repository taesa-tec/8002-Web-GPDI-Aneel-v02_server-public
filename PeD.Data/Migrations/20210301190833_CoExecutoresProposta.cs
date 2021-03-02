using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class CoExecutoresProposta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EmpresaRecebedoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaFinanciadoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaFinanciadoraId",
                table: "PropostaRecursosHumanosAlocacao",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "CoExecutorRecebedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRecursosHumanosAlocacao_PropostaCoExecutores_CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "CoExecutorFinanciadorId",
                principalTable: "PropostaCoExecutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaCoExecutores_CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "CoExecutorFinanciadorId",
                principalTable: "PropostaCoExecutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaCoExecutores_CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "CoExecutorRecebedorId",
                principalTable: "PropostaCoExecutores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRecursosHumanosAlocacao_PropostaCoExecutores_CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaCoExecutores_CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRecursosMateriaisAlocacao_PropostaCoExecutores_CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.DropColumn(
                name: "CoExecutorFinanciadorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropColumn(
                name: "CoExecutorRecebedorId",
                table: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropColumn(
                name: "CoExecutorFinanciadorId",
                table: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaRecebedoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaFinanciadoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaFinanciadoraId",
                table: "PropostaRecursosHumanosAlocacao",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
