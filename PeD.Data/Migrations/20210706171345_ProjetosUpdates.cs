using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class ProjetosUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Segmentos");

            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "Segmentos",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SegmentoId",
                table: "Projetos",
                nullable: true,
                defaultValue: "G");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos",
                column: "Valor");

            migrationBuilder.CreateTable(
                name: "ProjetosSubtemas",
                columns: table => new
                {
                    ProjetoId = table.Column<int>(nullable: false),
                    SubTemaId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Outro = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosSubtemas", x => new { x.ProjetoId, x.SubTemaId });
                    table.ForeignKey(
                        name: "FK_ProjetosSubtemas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosSubtemas_Temas_SubTemaId",
                        column: x => x.SubTemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_SegmentoId",
                table: "Projetos",
                column: "SegmentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosSubtemas_SubTemaId",
                table: "ProjetosSubtemas",
                column: "SubTemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Segmentos_SegmentoId",
                table: "Projetos",
                column: "SegmentoId",
                principalTable: "Segmentos",
                principalColumn: "Valor",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Segmentos_SegmentoId",
                table: "Projetos");

            migrationBuilder.DropTable(
                name: "ProjetosSubtemas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos");

            migrationBuilder.DropIndex(
                name: "IX_Projetos_SegmentoId",
                table: "Projetos");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "C");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "D");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "G");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "T");

            migrationBuilder.DropColumn(
                name: "SegmentoId",
                table: "Projetos");

            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "Segmentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Segmentos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Segmentos",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, "Geração", "G" },
                    { 2, "Transmissão", "T" },
                    { 3, "Distribuição", "D" },
                    { 4, "Comercialização", "C" }
                });
        }
    }
}
