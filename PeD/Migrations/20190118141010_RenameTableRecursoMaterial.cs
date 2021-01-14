using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Migrations
{
    public partial class RenameTableRecursoMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlocacoesRm_RecursoMaterais_RecursoMaterialId",
                table: "AlocacoesRm");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoMaterais_Projetos_ProjetoId",
                table: "RecursoMaterais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecursoMaterais",
                table: "RecursoMaterais");

            migrationBuilder.RenameTable(
                name: "RecursoMaterais",
                newName: "RecursoMateriais");

            migrationBuilder.RenameIndex(
                name: "IX_RecursoMaterais_ProjetoId",
                table: "RecursoMateriais",
                newName: "IX_RecursoMateriais_ProjetoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecursoMateriais",
                table: "RecursoMateriais",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlocacoesRm_RecursoMateriais_RecursoMaterialId",
                table: "AlocacoesRm",
                column: "RecursoMaterialId",
                principalTable: "RecursoMateriais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoMateriais_Projetos_ProjetoId",
                table: "RecursoMateriais",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlocacoesRm_RecursoMateriais_RecursoMaterialId",
                table: "AlocacoesRm");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoMateriais_Projetos_ProjetoId",
                table: "RecursoMateriais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecursoMateriais",
                table: "RecursoMateriais");

            migrationBuilder.RenameTable(
                name: "RecursoMateriais",
                newName: "RecursoMaterais");

            migrationBuilder.RenameIndex(
                name: "IX_RecursoMateriais_ProjetoId",
                table: "RecursoMaterais",
                newName: "IX_RecursoMaterais_ProjetoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecursoMaterais",
                table: "RecursoMaterais",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AlocacoesRm_RecursoMaterais_RecursoMaterialId",
                table: "AlocacoesRm",
                column: "RecursoMaterialId",
                principalTable: "RecursoMaterais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoMaterais_Projetos_ProjetoId",
                table: "RecursoMaterais",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
