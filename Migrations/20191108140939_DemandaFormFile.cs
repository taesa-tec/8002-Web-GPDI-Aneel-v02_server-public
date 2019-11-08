using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class DemandaFormFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandaFiles");

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Files",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "DemandaFormFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DemandaFormId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: true),
                    DemandaFormValuesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFormFiles_DemandaFormValues_DemandaFormValuesId",
                        column: x => x.DemandaFormValuesId,
                        principalTable: "DemandaFormValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandaFormFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormFiles_DemandaFormValuesId",
                table: "DemandaFormFiles",
                column: "DemandaFormValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormFiles_FileId",
                table: "DemandaFormFiles",
                column: "FileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemandaFormFiles");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Files");

            migrationBuilder.CreateTable(
                name: "DemandaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DemandaId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFiles_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandaFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFiles_DemandaId",
                table: "DemandaFiles",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFiles_FileId",
                table: "DemandaFiles",
                column: "FileId");
        }
    }
}
