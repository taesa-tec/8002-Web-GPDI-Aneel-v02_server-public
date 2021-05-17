using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class ComentariosFiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContratoComentarioFile",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoComentarioFile", x => new { x.ComentarioId, x.FileId });
                    table.ForeignKey(
                        name: "FK_ContratoComentarioFile_ContratoComentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "ContratoComentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratoComentarioFile_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanoComentarioFile",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoComentarioFile", x => new { x.ComentarioId, x.FileId });
                    table.ForeignKey(
                        name: "FK_PlanoComentarioFile_PlanoComentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "PlanoComentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanoComentarioFile_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarioFile_FileId",
                table: "ContratoComentarioFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarioFile_FileId",
                table: "PlanoComentarioFile",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContratoComentarioFile");

            migrationBuilder.DropTable(
                name: "PlanoComentarioFile");
        }
    }
}
