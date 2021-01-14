using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddRefpEUpload : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RegistrosFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: true),
                    Tipo = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    Mes = table.Column<string>(nullable: true),
                    QtdHrs = table.Column<string>(nullable: true),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: true),
                    TipoDocumento = table.Column<string>(nullable: true),
                    NumeroDocumento = table.Column<string>(nullable: true),
                    DataDocumento = table.Column<DateTime>(type: "date", nullable: false),
                    AtividadeRealizada = table.Column<string>(nullable: true),
                    NomeItem = table.Column<string>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    Beneficiado = table.Column<string>(nullable: true),
                    CnpjBeneficiado = table.Column<string>(nullable: true),
                    CategoriaContabil = table.Column<int>(nullable: false),
                    EquiparLabExistente = table.Column<bool>(nullable: false),
                    EquiparLabNovo = table.Column<bool>(nullable: false),
                    ItemNacional = table.Column<bool>(nullable: false),
                    QtdItens = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EspecificacaoTecnica = table.Column<string>(nullable: true),
                    FuncaoRecurso = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosFinanceiros_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosFinanceiros_Empresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosFinanceiros_RecursoHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "RecursoHumanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosFinanceiros_RecursoMateriais_RecursoMaterialId",
                        column: x => x.RecursoMaterialId,
                        principalTable: "RecursoMateriais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroObs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RegistroFinanceiroId = table.Column<int>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Texto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroObs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                        column: x => x.RegistroFinanceiroId,
                        principalTable: "RegistrosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistroObs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Uploads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NomeArquivo = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: true),
                    TemaId = table.Column<int>(nullable: true),
                    RegistroFinanceiroId = table.Column<int>(nullable: true),
                    Categoria = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uploads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uploads_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_RegistrosFinanceiros_RegistroFinanceiroId",
                        column: x => x.RegistroFinanceiroId,
                        principalTable: "RegistrosFinanceiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Uploads_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistroObs_RegistroFinanceiroId",
                table: "RegistroObs",
                column: "RegistroFinanceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroObs_UserId",
                table: "RegistroObs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_EmpresaFinanciadoraId",
                table: "RegistrosFinanceiros",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_EmpresaRecebedoraId",
                table: "RegistrosFinanceiros",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_RecursoHumanoId",
                table: "RegistrosFinanceiros",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_RecursoMaterialId",
                table: "RegistrosFinanceiros",
                column: "RecursoMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_ProjetoId",
                table: "Uploads",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_RegistroFinanceiroId",
                table: "Uploads",
                column: "RegistroFinanceiroId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_TemaId",
                table: "Uploads",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_UserId",
                table: "Uploads",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroObs");

            migrationBuilder.DropTable(
                name: "Uploads");

            migrationBuilder.DropTable(
                name: "RegistrosFinanceiros");
        }
    }
}
