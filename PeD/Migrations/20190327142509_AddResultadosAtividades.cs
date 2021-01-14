using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class AddResultadosAtividades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResApoioCitenel",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResBuscaAnterioridade",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResContratacaoAuditoria",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResDedicacaoHorario",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResDesenvFerramenta",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResDivulgacaoResultados",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResParticipacaoMembros",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResParticipacaoTecnicos",
                table: "AtividadesGestao",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResProspTecnologica",
                table: "AtividadesGestao",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResApoioCitenel",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResBuscaAnterioridade",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResContratacaoAuditoria",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResDedicacaoHorario",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResDesenvFerramenta",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResDivulgacaoResultados",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResParticipacaoMembros",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResParticipacaoTecnicos",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "ResProspTecnologica",
                table: "AtividadesGestao");
        }
    }
}
