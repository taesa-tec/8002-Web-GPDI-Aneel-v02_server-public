using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Projetos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    TituloCompleto = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    PropostaId = table.Column<int>(nullable: false),
                    Codigo = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    ProponenteId = table.Column<int>(nullable: true),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAlteracao = table.Column<DateTime>(nullable: false),
                    DataInicioProjeto = table.Column<DateTime>(nullable: false),
                    DataFinalProjeto = table.Column<DateTime>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false),
                    ResponsavelId = table.Column<string>(nullable: true),
                    CaptacaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projetos_Empresas_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projetos_Empresas_ProponenteId",
                        column: x => x.ProponenteId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projetos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projetos_AspNetUsers_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoArquivos",
                columns: table => new
                {
                    ProjetoId = table.Column<int>(nullable: false),
                    ArquivoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoArquivos", x => new { x.ProjetoId, x.ArquivoId });
                    table.ForeignKey(
                        name: "FK_ProjetoArquivos_Files_ArquivoId",
                        column: x => x.ArquivoId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoArquivos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoCoExecutores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    CNPJ = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Funcao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoCoExecutores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoCoExecutores_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoEscopos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Objetivo = table.Column<string>(nullable: true),
                    ResultadoEsperado = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_ProjetoEscopos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoEscopos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Objetivo = table.Column<string>(nullable: true),
                    Meses = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoMetas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoPlanosTrabalhos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Motivacao = table.Column<string>(nullable: true),
                    Originalidade = table.Column<string>(nullable: true),
                    Aplicabilidade = table.Column<string>(nullable: true),
                    Relevancia = table.Column<string>(nullable: true),
                    RazoabilidadeCustos = table.Column<string>(nullable: true),
                    MetodologiaTrabalho = table.Column<string>(nullable: true),
                    BuscaAnterioridade = table.Column<string>(nullable: true),
                    Bibliografia = table.Column<string>(nullable: true),
                    PesquisasCorrelatasPeDAneel = table.Column<string>(nullable: true),
                    PesquisasCorrelatasPeD = table.Column<string>(nullable: true),
                    PesquisasCorrelatasExecutora = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoPlanosTrabalhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoPlanosTrabalhos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Classificacao = table.Column<int>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    TipoId = table.Column<string>(nullable: true),
                    FaseCadeiaId = table.Column<string>(nullable: true),
                    TipoDetalhadoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoProdutos_FasesCadeiaProduto_FaseCadeiaId",
                        column: x => x.FaseCadeiaId,
                        principalTable: "FasesCadeiaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoProdutos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoProdutos_FaseTipoDetalhado_TipoDetalhadoId",
                        column: x => x.TipoDetalhadoId,
                        principalTable: "FaseTipoDetalhado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoProdutos_ProdutoTipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "ProdutoTipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoRecursosMateriais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    CategoriaContabilId = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    EspecificacaoTecnica = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoRecursosMateriais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriais_CategoriasContabeis_CategoriaContabilId",
                        column: x => x.CategoriaContabilId,
                        principalTable: "CategoriasContabeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriais_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoRiscos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Item = table.Column<string>(nullable: true),
                    Classificacao = table.Column<string>(nullable: true),
                    Justificativa = table.Column<string>(nullable: true),
                    Probabilidade = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoRiscos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRiscos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoRecursosHumanos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
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
                    table.PrimaryKey("PK_ProjetoRecursosHumanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanos_ProjetoCoExecutores_CoExecutorId",
                        column: x => x.CoExecutorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoEtapas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    DescricaoAtividades = table.Column<string>(nullable: true),
                    ProdutoId = table.Column<int>(nullable: false),
                    Meses = table.Column<string>(nullable: true),
                    Ordem = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoEtapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoEtapas_ProjetoProdutos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProjetoProdutos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoEtapas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoEtapasProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoEtapasProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoEtapasProdutos_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoEtapasProdutos_ProjetoProdutos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "ProjetoProdutos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoEtapasProdutos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoRecursosHumanosAlocacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: true),
                    CoExecutorFinanciadorId = table.Column<int>(nullable: true),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoId = table.Column<int>(nullable: false),
                    HoraMeses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoRecursosHumanosAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanosAlocacao_ProjetoCoExecutores_CoExecutorFinanciadorId",
                        column: x => x.CoExecutorFinanciadorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanosAlocacao_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanosAlocacao_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanosAlocacao_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanosAlocacao_ProjetoRecursosHumanos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "ProjetoRecursosHumanos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjetoRecursosMateriaisAlocacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: true),
                    CoExecutorFinanciadorId = table.Column<int>(nullable: true),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoId = table.Column<int>(nullable: false),
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    CoExecutorRecebedorId = table.Column<int>(nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoRecursosMateriaisAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_ProjetoCoExecutores_CoExecutorFinanciadorId",
                        column: x => x.CoExecutorFinanciadorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_ProjetoCoExecutores_CoExecutorRecebedorId",
                        column: x => x.CoExecutorRecebedorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_Empresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosMateriaisAlocacao_ProjetoRecursosMateriais_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "ProjetoRecursosMateriais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoArquivos_ArquivoId",
                table: "ProjetoArquivos",
                column: "ArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoCoExecutores_ProjetoId",
                table: "ProjetoCoExecutores",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEscopos_ProjetoId",
                table: "ProjetoEscopos",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEtapas_ProdutoId",
                table: "ProjetoEtapas",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEtapas_ProjetoId",
                table: "ProjetoEtapas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEtapasProdutos_EtapaId",
                table: "ProjetoEtapasProdutos",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEtapasProdutos_ProdutoId",
                table: "ProjetoEtapasProdutos",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEtapasProdutos_ProjetoId",
                table: "ProjetoEtapasProdutos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoMetas_ProjetoId",
                table: "ProjetoMetas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoPlanosTrabalhos_ProjetoId",
                table: "ProjetoPlanosTrabalhos",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoProdutos_FaseCadeiaId",
                table: "ProjetoProdutos",
                column: "FaseCadeiaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoProdutos_ProjetoId",
                table: "ProjetoProdutos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoProdutos_TipoDetalhadoId",
                table: "ProjetoProdutos",
                column: "TipoDetalhadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoProdutos_TipoId",
                table: "ProjetoProdutos",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanos_CoExecutorId",
                table: "ProjetoRecursosHumanos",
                column: "CoExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanos_EmpresaId",
                table: "ProjetoRecursosHumanos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanos_ProjetoId",
                table: "ProjetoRecursosHumanos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanosAlocacao_CoExecutorFinanciadorId",
                table: "ProjetoRecursosHumanosAlocacao",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanosAlocacao_EmpresaFinanciadoraId",
                table: "ProjetoRecursosHumanosAlocacao",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanosAlocacao_EtapaId",
                table: "ProjetoRecursosHumanosAlocacao",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanosAlocacao_ProjetoId",
                table: "ProjetoRecursosHumanosAlocacao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosHumanosAlocacao_RecursoId",
                table: "ProjetoRecursosHumanosAlocacao",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriais_CategoriaContabilId",
                table: "ProjetoRecursosMateriais",
                column: "CategoriaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriais_ProjetoId",
                table: "ProjetoRecursosMateriais",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_CoExecutorFinanciadorId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_CoExecutorRecebedorId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "CoExecutorRecebedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_EmpresaFinanciadoraId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_EmpresaRecebedoraId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_EtapaId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_ProjetoId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriaisAlocacao_RecursoId",
                table: "ProjetoRecursosMateriaisAlocacao",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRiscos_ProjetoId",
                table: "ProjetoRiscos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_CaptacaoId",
                table: "Projetos",
                column: "CaptacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_FornecedorId",
                table: "Projetos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_ProponenteId",
                table: "Projetos",
                column: "ProponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_PropostaId",
                table: "Projetos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_ResponsavelId",
                table: "Projetos",
                column: "ResponsavelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjetoArquivos");

            migrationBuilder.DropTable(
                name: "ProjetoEscopos");

            migrationBuilder.DropTable(
                name: "ProjetoEtapasProdutos");

            migrationBuilder.DropTable(
                name: "ProjetoMetas");

            migrationBuilder.DropTable(
                name: "ProjetoPlanosTrabalhos");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosHumanosAlocacao");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosMateriaisAlocacao");

            migrationBuilder.DropTable(
                name: "ProjetoRiscos");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosHumanos");

            migrationBuilder.DropTable(
                name: "ProjetoEtapas");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosMateriais");

            migrationBuilder.DropTable(
                name: "ProjetoCoExecutores");

            migrationBuilder.DropTable(
                name: "ProjetoProdutos");

            migrationBuilder.DropTable(
                name: "Projetos");
        }
    }
}
