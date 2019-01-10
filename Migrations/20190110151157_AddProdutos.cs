using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddProdutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aplicabilidade",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvaliacaoInicial",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompartResultados",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Projetos",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "Motivacao",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Originalidade",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pesquisas",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Razoabilidade",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Relevancia",
                table: "Projetos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SegmentoId",
                table: "Projetos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Projetos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produto_ProjetoId",
                table: "Produto",
                column: "ProjetoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropColumn(
                name: "Aplicabilidade",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "AvaliacaoInicial",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "CompartResultados",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Motivacao",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Originalidade",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Pesquisas",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Razoabilidade",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Relevancia",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "SegmentoId",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Projetos");
        }
    }
}
