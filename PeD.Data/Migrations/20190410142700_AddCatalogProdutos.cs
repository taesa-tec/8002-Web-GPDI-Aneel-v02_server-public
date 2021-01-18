using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddCatalogProdutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EtapaProdutos_Produtos_ProdutoId",
                table: "EtapaProdutos");

            migrationBuilder.DropIndex(
                name: "IX_EtapaProdutos_ProdutoId",
                table: "EtapaProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "FaseCadeia",
                table: "Produtos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CatalogProdutoFaseCadeiaId",
                table: "Produtos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogProdutoTipoDetalhadoId",
                table: "Produtos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProdutoFaseCadeia",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProdutoFaseCadeia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogProdutoTipoDetalhado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CatalogProdutoFaseCadeiaId = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogProdutoTipoDetalhado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogProdutoTipoDetalhado_CatalogProdutoFaseCadeia_CatalogProdutoFaseCadeiaId",
                        column: x => x.CatalogProdutoFaseCadeiaId,
                        principalTable: "ProdutoFaseCadeia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CatalogProdutoFaseCadeiaId",
                table: "Produtos",
                column: "CatalogProdutoFaseCadeiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CatalogProdutoTipoDetalhadoId",
                table: "Produtos",
                column: "CatalogProdutoTipoDetalhadoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtividadesGestao_ProjetoId",
                table: "AtividadesGestao",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogProdutoTipoDetalhado_CatalogProdutoFaseCadeiaId",
                table: "CatalogProdutoTipoDetalhado",
                column: "CatalogProdutoFaseCadeiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AtividadesGestao_Projetos_ProjetoId",
                table: "AtividadesGestao",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_CatalogProdutoFaseCadeia_CatalogProdutoFaseCadeiaId",
                table: "Produtos",
                column: "CatalogProdutoFaseCadeiaId",
                principalTable: "ProdutoFaseCadeia",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_CatalogProdutoTipoDetalhado_CatalogProdutoTipoDetalhadoId",
                table: "Produtos",
                column: "CatalogProdutoTipoDetalhadoId",
                principalTable: "CatalogProdutoTipoDetalhado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AtividadesGestao_Projetos_ProjetoId",
                table: "AtividadesGestao");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_CatalogProdutoFaseCadeia_CatalogProdutoFaseCadeiaId",
                table: "Produtos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_CatalogProdutoTipoDetalhado_CatalogProdutoTipoDetalhadoId",
                table: "Produtos");

            migrationBuilder.DropTable(
                name: "CatalogProdutoTipoDetalhado");

            migrationBuilder.DropTable(
                name: "ProdutoFaseCadeia");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CatalogProdutoFaseCadeiaId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CatalogProdutoTipoDetalhadoId",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_AtividadesGestao_ProjetoId",
                table: "AtividadesGestao");

            migrationBuilder.DropColumn(
                name: "CatalogProdutoFaseCadeiaId",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "CatalogProdutoTipoDetalhadoId",
                table: "Produtos");

            migrationBuilder.AlterColumn<int>(
                name: "FaseCadeia",
                table: "Produtos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EtapaProdutos_ProdutoId",
                table: "EtapaProdutos",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EtapaProdutos_Produtos_ProdutoId",
                table: "EtapaProdutos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
