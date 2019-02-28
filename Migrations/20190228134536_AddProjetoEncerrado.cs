using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace APIGestor.Migrations
{
    public partial class AddProjetoEncerrado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatorioFinalId",
                table: "Uploads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResultadoCapacitacaoId",
                table: "Uploads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResultadoProducaoId",
                table: "Uploads",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AtividadesRealizadas",
                table: "Etapas",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatalogPaises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogPaises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatorioFinal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProdutoAlcancado = table.Column<bool>(nullable: true),
                    JustificativaProduto = table.Column<string>(nullable: true),
                    EspecificacaoProduto = table.Column<string>(nullable: true),
                    TecnicaPrevista = table.Column<bool>(nullable: true),
                    JustificativaTecnica = table.Column<string>(nullable: true),
                    DescTecnica = table.Column<string>(nullable: true),
                    AplicabilidadePrevista = table.Column<bool>(nullable: true),
                    JustificativaAplicabilidade = table.Column<string>(nullable: true),
                    DescTestes = table.Column<string>(nullable: true),
                    DescAbrangencia = table.Column<string>(nullable: true),
                    DescAmbito = table.Column<string>(nullable: true),
                    DescAtividades = table.Column<string>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatorioFinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosCapacitacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    RecursoHumanoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Conclusao = table.Column<bool>(nullable: true),
                    DataConclusao = table.Column<DateTime>(nullable: false),
                    CnpjInstituicao = table.Column<string>(nullable: true),
                    AreaPesquisa = table.Column<string>(nullable: true),
                    TituloTrabalho = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosCapacitacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosCapacitacao_RecursoHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "RecursoHumanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosEconomico",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    UnidadeBase = table.Column<string>(nullable: true),
                    ValorIndicador = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    Percentagem = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    ValorBeneficio = table.Column<decimal>(type: "decimal(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosEconomico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosInfra",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    CnpjReceptora = table.Column<string>(nullable: true),
                    NomeLaboratorio = table.Column<string>(nullable: true),
                    AreaPesquisa = table.Column<string>(nullable: true),
                    ListaMateriais = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosInfra", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosIntelectual",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    DataPedido = table.Column<DateTime>(nullable: false),
                    NumeroPedido = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosIntelectual", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosSocioAmbiental",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Positivo = table.Column<bool>(nullable: true),
                    TecnicaPrevista = table.Column<bool>(nullable: true),
                    Desc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosSocioAmbiental", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResultadosProducao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    DataPublicacao = table.Column<DateTime>(nullable: false),
                    Confirmacao = table.Column<bool>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    CatalogPaisId = table.Column<int>(nullable: false),
                    Cidade = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosProducao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosProducao_CatalogPaises_CatalogPaisId",
                        column: x => x.CatalogPaisId,
                        principalTable: "CatalogPaises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoIntelectualDepositantes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ResultadoIntelectualId = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    Entidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoIntelectualDepositantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoIntelectualDepositantes_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadoIntelectualDepositantes_ResultadosIntelectual_ResultadoIntelectualId",
                        column: x => x.ResultadoIntelectualId,
                        principalTable: "ResultadosIntelectual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResultadoIntelectualInventores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ResultadoIntelectualId = table.Column<int>(nullable: false),
                    RecursoHumanoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoIntelectualInventores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoIntelectualInventores_RecursoHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "RecursoHumanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResultadoIntelectualInventores_ResultadosIntelectual_ResultadoIntelectualId",
                        column: x => x.ResultadoIntelectualId,
                        principalTable: "ResultadosIntelectual",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_RelatorioFinalId",
                table: "Uploads",
                column: "RelatorioFinalId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_ResultadoCapacitacaoId",
                table: "Uploads",
                column: "ResultadoCapacitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_ResultadoProducaoId",
                table: "Uploads",
                column: "ResultadoProducaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoIntelectualDepositantes_EmpresaId",
                table: "ResultadoIntelectualDepositantes",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoIntelectualDepositantes_ResultadoIntelectualId",
                table: "ResultadoIntelectualDepositantes",
                column: "ResultadoIntelectualId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoIntelectualInventores_RecursoHumanoId",
                table: "ResultadoIntelectualInventores",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoIntelectualInventores_ResultadoIntelectualId",
                table: "ResultadoIntelectualInventores",
                column: "ResultadoIntelectualId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCapacitacao_RecursoHumanoId",
                table: "ResultadosCapacitacao",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosProducao_CatalogPaisId",
                table: "ResultadosProducao",
                column: "CatalogPaisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_RelatorioFinal_RelatorioFinalId",
                table: "Uploads",
                column: "RelatorioFinalId",
                principalTable: "RelatorioFinal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_ResultadosCapacitacao_ResultadoCapacitacaoId",
                table: "Uploads",
                column: "ResultadoCapacitacaoId",
                principalTable: "ResultadosCapacitacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_ResultadosProducao_ResultadoProducaoId",
                table: "Uploads",
                column: "ResultadoProducaoId",
                principalTable: "ResultadosProducao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_RelatorioFinal_RelatorioFinalId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_ResultadosCapacitacao_ResultadoCapacitacaoId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_ResultadosProducao_ResultadoProducaoId",
                table: "Uploads");

            migrationBuilder.DropTable(
                name: "RelatorioFinal");

            migrationBuilder.DropTable(
                name: "ResultadoIntelectualDepositantes");

            migrationBuilder.DropTable(
                name: "ResultadoIntelectualInventores");

            migrationBuilder.DropTable(
                name: "ResultadosCapacitacao");

            migrationBuilder.DropTable(
                name: "ResultadosEconomico");

            migrationBuilder.DropTable(
                name: "ResultadosInfra");

            migrationBuilder.DropTable(
                name: "ResultadosProducao");

            migrationBuilder.DropTable(
                name: "ResultadosSocioAmbiental");

            migrationBuilder.DropTable(
                name: "ResultadosIntelectual");

            migrationBuilder.DropTable(
                name: "CatalogPaises");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_RelatorioFinalId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_ResultadoCapacitacaoId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_ResultadoProducaoId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "RelatorioFinalId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "ResultadoCapacitacaoId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "ResultadoProducaoId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "AtividadesRealizadas",
                table: "Etapas");
        }
    }
}
