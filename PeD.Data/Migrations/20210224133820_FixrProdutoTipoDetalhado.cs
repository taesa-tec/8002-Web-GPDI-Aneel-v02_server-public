using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class FixrProdutoTipoDetalhado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaProdutos_FasesCadeiaProduto_FaseCadeiaId1",
                table: "PropostaProdutos");

            migrationBuilder.DropIndex(
                name: "IX_PropostaProdutos_FaseCadeiaId1",
                table: "PropostaProdutos");

            migrationBuilder.DropColumn(
                name: "FaseCadeiaId1",
                table: "PropostaProdutos");

            migrationBuilder.DropColumn(
                name: "TipoDetalhado",
                table: "PropostaProdutos");

            migrationBuilder.AlterColumn<string>(
                name: "FaseCadeiaId",
                table: "PropostaProdutos",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TipoDetalhadoId",
                table: "PropostaProdutos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_FaseCadeiaId",
                table: "PropostaProdutos",
                column: "FaseCadeiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_TipoDetalhadoId",
                table: "PropostaProdutos",
                column: "TipoDetalhadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaProdutos_FasesCadeiaProduto_FaseCadeiaId",
                table: "PropostaProdutos",
                column: "FaseCadeiaId",
                principalTable: "FasesCadeiaProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaProdutos_FaseTipoDetalhado_TipoDetalhadoId",
                table: "PropostaProdutos",
                column: "TipoDetalhadoId",
                principalTable: "FaseTipoDetalhado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropostaProdutos_FasesCadeiaProduto_FaseCadeiaId",
                table: "PropostaProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaProdutos_FaseTipoDetalhado_TipoDetalhadoId",
                table: "PropostaProdutos");

            migrationBuilder.DropIndex(
                name: "IX_PropostaProdutos_FaseCadeiaId",
                table: "PropostaProdutos");

            migrationBuilder.DropIndex(
                name: "IX_PropostaProdutos_TipoDetalhadoId",
                table: "PropostaProdutos");

            migrationBuilder.DropColumn(
                name: "TipoDetalhadoId",
                table: "PropostaProdutos");

            migrationBuilder.AlterColumn<int>(
                name: "FaseCadeiaId",
                table: "PropostaProdutos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FaseCadeiaId1",
                table: "PropostaProdutos",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoDetalhado",
                table: "PropostaProdutos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_FaseCadeiaId1",
                table: "PropostaProdutos",
                column: "FaseCadeiaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaProdutos_FasesCadeiaProduto_FaseCadeiaId1",
                table: "PropostaProdutos",
                column: "FaseCadeiaId1",
                principalTable: "FasesCadeiaProduto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
