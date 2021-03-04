using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class CaptacaoTemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.AddColumn<int>(
                name: "TemaId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TemaOutro",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaptacaoSubTemas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outro = table.Column<string>(nullable: true),
                    CaptacaoId = table.Column<int>(nullable: false),
                    SubTemaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoSubTemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaptacaoSubTemas_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoSubTemas_Temas_SubTemaId",
                        column: x => x.SubTemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_TemaId",
                table: "Captacoes",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSubTemas_CaptacaoId",
                table: "CaptacaoSubTemas",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSubTemas_SubTemaId",
                table: "CaptacaoSubTemas",
                column: "SubTemaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Temas_TemaId",
                table: "Captacoes",
                column: "TemaId",
                principalTable: "Temas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Temas_TemaId",
                table: "Captacoes");

            migrationBuilder.DropTable(
                name: "CaptacaoSubTemas");

            migrationBuilder.DropIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_TemaId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "TemaId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "TemaOutro",
                table: "Captacoes");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos",
                column: "PropostaId");
        }
    }
}
