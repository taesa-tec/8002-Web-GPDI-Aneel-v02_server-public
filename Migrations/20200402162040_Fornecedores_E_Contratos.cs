using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class Fornecedores_E_Contratos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clausulas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordem = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clausulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "contratos",
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
                    table.PrimaryKey("PK_contratos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fornecedores",
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
                    table.PrimaryKey("PK_fornecedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fornecedores_AspNetUsers_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "coexecutores",
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
                    table.PrimaryKey("PK_coexecutores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coexecutores_fornecedores_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "fornecedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_coexecutores_FornecedorId",
                table: "coexecutores",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_fornecedores_ResponsavelId",
                table: "fornecedores",
                column: "ResponsavelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clausulas");

            migrationBuilder.DropTable(
                name: "coexecutores");

            migrationBuilder.DropTable(
                name: "contratos");

            migrationBuilder.DropTable(
                name: "fornecedores");
        }
    }
}
