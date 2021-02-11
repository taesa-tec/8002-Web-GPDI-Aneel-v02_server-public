using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class PropostasChilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropostaCoExecutores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    CNPJ = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaCoExecutores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaCoExecutores_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaContratos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true),
                    Finalizado = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaContratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaContratos_Contratos_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaContratos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaEscopos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Objetivo = table.Column<string>(nullable: true),
                    BeneficioTaesa = table.Column<string>(nullable: true),
                    BeneficioInstitucional = table.Column<string>(nullable: true),
                    BeneficioIndustria = table.Column<string>(nullable: true),
                    BeneficioSetorEletrico = table.Column<string>(nullable: true),
                    BeneficioSociedade = table.Column<string>(nullable: true),
                    ExperienciaPrevia = table.Column<string>(nullable: true),
                    Contrapartidas = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaEscopos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEscopos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaEtapas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    DescricaoAtividades = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaEtapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEtapas_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaPlanosTrabalhos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Motivacao = table.Column<string>(nullable: true),
                    Originalidade = table.Column<string>(nullable: true),
                    Aplicabilidade = table.Column<string>(nullable: true),
                    Relevancia = table.Column<string>(nullable: true),
                    RazoabilidadeCustos = table.Column<string>(nullable: true),
                    PesquisasCorrelatas = table.Column<string>(nullable: true),
                    MetodologiaTrabalho = table.Column<string>(nullable: true),
                    BuscaAnterioridade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaPlanosTrabalhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaPlanosTrabalhos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Classificacao = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    FaseCadeia = table.Column<int>(nullable: false),
                    TipoDetalhado = table.Column<string>(nullable: true),
                    Titulo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaProdutos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaRecursosMateriais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    CategoriaContabilId = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EspecificacaoTecnica = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosMateriais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriais_CategoriasContabeis_CategoriaContabilId",
                        column: x => x.CategoriaContabilId,
                        principalTable: "CategoriasContabeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriais_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaRiscos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Item = table.Column<string>(nullable: true),
                    Classificacao = table.Column<string>(nullable: true),
                    Justificativa = table.Column<string>(nullable: true),
                    Probabilidade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRiscos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRiscos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaRecursosHumanos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    NomeCompleto = table.Column<string>(nullable: true),
                    Titulacao = table.Column<string>(nullable: true),
                    Funcao = table.Column<string>(nullable: true),
                    Nacionalidade = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    Documento = table.Column<string>(nullable: true),
                    ValorHora = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    UrlCurriculo = table.Column<string>(nullable: true),
                    CoExecutorId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosHumanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanos_PropostaCoExecutores_CoExecutorId",
                        column: x => x.CoExecutorId,
                        principalTable: "PropostaCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaContratosRevisao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    ContratoId = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaContratosRevisao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaContratosRevisao_PropostaContratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "PropostaContratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaContratosRevisao_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropostaEtapasProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaEtapasProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEtapasProdutos_PropostaEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "PropostaEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaEtapasProdutos_PropostaProdutos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PropostaProdutos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaEtapasProdutos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaRecursosMateriaisAlocacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoId = table.Column<int>(nullable: false),
                    EmpresaRecebedoraId = table.Column<int>(nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosMateriaisAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_Empresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "PropostaEtapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_PropostaRecursosMateriais_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "PropostaRecursosMateriais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropostaRecursosHumanosAlocacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosHumanosAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanosAlocacao_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanosAlocacao_PropostaEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "PropostaEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanosAlocacao_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanosAlocacao_PropostaRecursosHumanos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "PropostaRecursosHumanos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropostaCoExecutores_PropostaId",
                table: "PropostaCoExecutores",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_ParentId",
                table: "PropostaContratos",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratosRevisao_ContratoId",
                table: "PropostaContratosRevisao",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratosRevisao_PropostaId",
                table: "PropostaContratosRevisao",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapas_PropostaId",
                table: "PropostaEtapas",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapasProdutos_EtapaId",
                table: "PropostaEtapasProdutos",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapasProdutos_ProdutoId",
                table: "PropostaEtapasProdutos",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapasProdutos_PropostaId",
                table: "PropostaEtapasProdutos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_PropostaId",
                table: "PropostaProdutos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanos_CoExecutorId",
                table: "PropostaRecursosHumanos",
                column: "CoExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanos_EmpresaId",
                table: "PropostaRecursosHumanos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanos_PropostaId",
                table: "PropostaRecursosHumanos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_EmpresaFinanciadoraId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_EtapaId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_PropostaId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosHumanosAlocacao_RecursoId",
                table: "PropostaRecursosHumanosAlocacao",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriais_CategoriaContabilId",
                table: "PropostaRecursosMateriais",
                column: "CategoriaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriais_PropostaId",
                table: "PropostaRecursosMateriais",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_EmpresaFinanciadoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_EmpresaRecebedoraId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_EtapaId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_PropostaId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRecursosMateriaisAlocacao_RecursoId",
                table: "PropostaRecursosMateriaisAlocacao",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRiscos_PropostaId",
                table: "PropostaRiscos",
                column: "PropostaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropostaContratosRevisao");

            migrationBuilder.DropTable(
                name: "PropostaEscopos");

            migrationBuilder.DropTable(
                name: "PropostaEtapasProdutos");

            migrationBuilder.DropTable(
                name: "PropostaPlanosTrabalhos");

            migrationBuilder.DropTable(
                name: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.DropTable(
                name: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropTable(
                name: "PropostaRiscos");

            migrationBuilder.DropTable(
                name: "PropostaContratos");

            migrationBuilder.DropTable(
                name: "PropostaProdutos");

            migrationBuilder.DropTable(
                name: "PropostaRecursosHumanos");

            migrationBuilder.DropTable(
                name: "PropostaEtapas");

            migrationBuilder.DropTable(
                name: "PropostaRecursosMateriais");

            migrationBuilder.DropTable(
                name: "PropostaCoExecutores");
        }
    }
}
