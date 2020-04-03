using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class Fornecedores_E_Contratos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clausulas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordem = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clausulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    Conteudo = table.Column<string>(nullable: true),
                    Tipo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    ResponsavelId = table.Column<string>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fornecedores_AspNetUsers_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoExecutores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FornecedorId = table.Column<int>(nullable: false),
                    CNPJ = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoExecutores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoExecutores_Fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoExecutores_FornecedorId",
                table: "CoExecutores",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_ResponsavelId",
                table: "Fornecedores",
                column: "ResponsavelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clausulas");

            migrationBuilder.DropTable(
                name: "CoExecutores");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Fornecedores");
        }
    }
}
