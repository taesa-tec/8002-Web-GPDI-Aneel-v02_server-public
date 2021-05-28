using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class RegistrosFinanceiros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjetosRegistrosFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    FinanciadoraId = table.Column<int>(nullable: true),
                    CoExecutorFinanciadorId = table.Column<int>(nullable: true),
                    MesReferencia = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    TipoDocumento = table.Column<string>(nullable: false),
                    NumeroDocumento = table.Column<string>(nullable: true),
                    DataDocumento = table.Column<DateTime>(nullable: false),
                    ObservacaoInterna = table.Column<string>(nullable: true),
                    ComprovanteId = table.Column<int>(nullable: true),
                    AtividadeRealizada = table.Column<string>(nullable: true),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    NomeItem = table.Column<string>(nullable: true),
                    Beneficiado = table.Column<string>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    CnpjBeneficiado = table.Column<string>(nullable: true),
                    CategoriaContabilId = table.Column<int>(nullable: true),
                    EquipaLaboratorioExistente = table.Column<bool>(nullable: true),
                    EquipaLaboratorioNovo = table.Column<bool>(nullable: true),
                    IsNacional = table.Column<bool>(nullable: true),
                    Quantidade = table.Column<int>(nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    EspecificaoTecnica = table.Column<string>(nullable: true),
                    FuncaoEtapa = table.Column<string>(nullable: true),
                    RecebedoraId = table.Column<int>(nullable: true),
                    CoExecutorRecebedorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRegistrosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoCoExecutores_CoExecutorFinanciadorId",
                        column: x => x.CoExecutorFinanciadorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_Files_ComprovanteId",
                        column: x => x.ComprovanteId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_Empresas_FinanciadoraId",
                        column: x => x.FinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoRecursosHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "ProjetoRecursosHumanos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_CategoriasContabeis_CategoriaContabilId",
                        column: x => x.CategoriaContabilId,
                        principalTable: "CategoriasContabeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoCoExecutores_CoExecutorRecebedorId",
                        column: x => x.CoExecutorRecebedorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_Empresas_RecebedoraId",
                        column: x => x.RecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoRecursosMateriais_RecursoMaterialId",
                        column: x => x.RecursoMaterialId,
                        principalTable: "ProjetoRecursosMateriais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_CoExecutorFinanciadorId",
                table: "ProjetosRegistrosFinanceiros",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_ComprovanteId",
                table: "ProjetosRegistrosFinanceiros",
                column: "ComprovanteId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_EtapaId",
                table: "ProjetosRegistrosFinanceiros",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_FinanciadoraId",
                table: "ProjetosRegistrosFinanceiros",
                column: "FinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_ProjetoId",
                table: "ProjetosRegistrosFinanceiros",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_RecursoHumanoId",
                table: "ProjetosRegistrosFinanceiros",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_CategoriaContabilId",
                table: "ProjetosRegistrosFinanceiros",
                column: "CategoriaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_CoExecutorRecebedorId",
                table: "ProjetosRegistrosFinanceiros",
                column: "CoExecutorRecebedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_RecebedoraId",
                table: "ProjetosRegistrosFinanceiros",
                column: "RecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_RecursoMaterialId",
                table: "ProjetosRegistrosFinanceiros",
                column: "RecursoMaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjetosRegistrosFinanceiros");
        }
    }
}
