using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Projetos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Captacoes SET UsuarioExecucaoId = null, IsProjetoAprovado = null, ArquivoFormalizacaoId = null");
            migrationBuilder.AddColumn<int>(
                name: "EspecificacaoTecnicaFileId",
                table: "Demandas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EspecificacaoTecnicaFileId",
                table: "Captacoes",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    TituloCompleto = table.Column<string>(nullable: true),
                    PlanoTrabalhoFileId = table.Column<int>(nullable: false),
                    ContratoId = table.Column<int>(nullable: false),
                    EspecificacaoTecnicaFileId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TemaId = table.Column<int>(nullable: true),
                    TemaOutro = table.Column<string>(nullable: true),
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
                        name: "FK_Projetos_Files_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projetos_Files_EspecificacaoTecnicaFileId",
                        column: x => x.EspecificacaoTecnicaFileId,
                        principalTable: "Files",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projetos_Empresas_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Projetos_Files_PlanoTrabalhoFileId",
                        column: x => x.PlanoTrabalhoFileId,
                        principalTable: "Files",
                        principalColumn: "Id");
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
                    table.ForeignKey(
                        name: "FK_Projetos_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
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
                name: "ProjetosRecursosAlocacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: true),
                    CoExecutorFinanciadorId = table.Column<int>(nullable: true),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    CoExecutorRecebedorId = table.Column<int>(nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRecursosAlocacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoCoExecutores_CoExecutorFinanciadorId",
                        column: x => x.CoExecutorFinanciadorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoRecursosHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "ProjetoRecursosHumanos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoCoExecutores_CoExecutorRecebedorId",
                        column: x => x.CoExecutorRecebedorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_Empresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoRecursosMateriais_RecursoMaterialId",
                        column: x => x.RecursoMaterialId,
                        principalTable: "ProjetoRecursosMateriais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRegistrosFinanceiros",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    FinanciadoraId = table.Column<int>(nullable: true),
                    CoExecutorFinanciadorId = table.Column<int>(nullable: true),
                    MesReferencia = table.Column<DateTime>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    TipoDocumento = table.Column<string>(nullable: false),
                    NumeroDocumento = table.Column<string>(nullable: true),
                    DataDocumento = table.Column<DateTime>(nullable: false),
                    ComprovanteId = table.Column<int>(nullable: true),
                    AtividadeRealizada = table.Column<string>(nullable: true),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    Horas = table.Column<decimal>(type: "decimal(18, 2)", nullable: true),
                    NomeItem = table.Column<string>(nullable: true),
                    Beneficiado = table.Column<string>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    CnpjBeneficiado = table.Column<string>(nullable: true),
                    CategoriaContabilId = table.Column<int>(nullable: true),
                    EquipaLaboratorioExistente = table.Column<bool>(nullable: true),
                    EquipaLaboratorioNovo = table.Column<bool>(nullable: true),
                    IsNacional = table.Column<bool>(nullable: true),
                    Quantidade = table.Column<int>(nullable: true),
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

            migrationBuilder.CreateTable(
                name: "ProjetosAlocacaoRhHorasMeses",
                columns: table => new
                {
                    AlocacaoRhId = table.Column<int>(nullable: false),
                    Mes = table.Column<int>(nullable: false),
                    Horas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosAlocacaoRhHorasMeses", x => new { x.AlocacaoRhId, x.Mes });
                    table.ForeignKey(
                        name: "FK_ProjetosAlocacaoRhHorasMeses_ProjetosRecursosAlocacoes_AlocacaoRhId",
                        column: x => x.AlocacaoRhId,
                        principalTable: "ProjetosRecursosAlocacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRegistrosFinanceirosObservacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    RegistroId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRegistrosFinanceirosObservacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceirosObservacoes_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceirosObservacoes_ProjetosRegistrosFinanceiros_RegistroId",
                        column: x => x.RegistroId,
                        principalTable: "ProjetosRegistrosFinanceiros",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_EspecificacaoTecnicaFileId",
                table: "Demandas",
                column: "EspecificacaoTecnicaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_EspecificacaoTecnicaFileId",
                table: "Captacoes",
                column: "EspecificacaoTecnicaFileId",
                unique: true,
                filter: "[EspecificacaoTecnicaFileId] IS NOT NULL");

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
                name: "IX_ProjetoRecursosMateriais_CategoriaContabilId",
                table: "ProjetoRecursosMateriais",
                column: "CategoriaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoRecursosMateriais_ProjetoId",
                table: "ProjetoRecursosMateriais",
                column: "ProjetoId");

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
                name: "IX_Projetos_ContratoId",
                table: "Projetos",
                column: "ContratoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_EspecificacaoTecnicaFileId",
                table: "Projetos",
                column: "EspecificacaoTecnicaFileId",
                unique: true,
                filter: "[EspecificacaoTecnicaFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_FornecedorId",
                table: "Projetos",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_PlanoTrabalhoFileId",
                table: "Projetos",
                column: "PlanoTrabalhoFileId",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_TemaId",
                table: "Projetos",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_CoExecutorFinanciadorId",
                table: "ProjetosRecursosAlocacoes",
                column: "CoExecutorFinanciadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_EmpresaFinanciadoraId",
                table: "ProjetosRecursosAlocacoes",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_EtapaId",
                table: "ProjetosRecursosAlocacoes",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_ProjetoId",
                table: "ProjetosRecursosAlocacoes",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_RecursoHumanoId",
                table: "ProjetosRecursosAlocacoes",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_CoExecutorRecebedorId",
                table: "ProjetosRecursosAlocacoes",
                column: "CoExecutorRecebedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_EmpresaRecebedoraId",
                table: "ProjetosRecursosAlocacoes",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_RecursoMaterialId",
                table: "ProjetosRecursosAlocacoes",
                column: "RecursoMaterialId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceirosObservacoes_AuthorId",
                table: "ProjetosRegistrosFinanceirosObservacoes",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceirosObservacoes_RegistroId",
                table: "ProjetosRegistrosFinanceirosObservacoes",
                column: "RegistroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_EspecificacaoTecnicaFileId",
                table: "Captacoes",
                column: "EspecificacaoTecnicaFileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_Files_EspecificacaoTecnicaFileId",
                table: "Demandas",
                column: "EspecificacaoTecnicaFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_Files_EspecificacaoTecnicaFileId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_Files_EspecificacaoTecnicaFileId",
                table: "Demandas");

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
                name: "ProjetoRiscos");

            migrationBuilder.DropTable(
                name: "ProjetosAlocacaoRhHorasMeses");

            migrationBuilder.DropTable(
                name: "ProjetosRegistrosFinanceirosObservacoes");

            migrationBuilder.DropTable(
                name: "ProjetosRecursosAlocacoes");

            migrationBuilder.DropTable(
                name: "ProjetosRegistrosFinanceiros");

            migrationBuilder.DropTable(
                name: "ProjetoEtapas");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosHumanos");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosMateriais");

            migrationBuilder.DropTable(
                name: "ProjetoProdutos");

            migrationBuilder.DropTable(
                name: "ProjetoCoExecutores");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropIndex(
                name: "IX_Demandas_EspecificacaoTecnicaFileId",
                table: "Demandas");

            migrationBuilder.DropIndex(
                name: "IX_Captacoes_EspecificacaoTecnicaFileId",
                table: "Captacoes");

            migrationBuilder.DropColumn(
                name: "EspecificacaoTecnicaFileId",
                table: "Demandas");

            migrationBuilder.DropColumn(
                name: "EspecificacaoTecnicaFileId",
                table: "Captacoes");
        }
    }
}
