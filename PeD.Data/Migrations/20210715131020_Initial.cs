using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasContabeis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasContabeis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clausulas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordem = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clausulas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contratos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    Header = table.Column<string>(nullable: true),
                    Conteudo = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contratos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemandaFormValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandaId = table.Column<int>(nullable: false),
                    FormKey = table.Column<string>(nullable: true),
                    Revisao = table.Column<int>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(type: "varchar(max)", nullable: true),
                    Html = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormValues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FasesCadeiaProduto",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasesCadeiaProduto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemAjuda",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Conteudo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAjuda", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoTipos",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoTipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Segmentos",
                columns: table => new
                {
                    Valor = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segmentos", x => x.Valor);
                });

            migrationBuilder.CreateTable(
                name: "SystemOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Temas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temas_Temas_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaContabilAtividades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true),
                    CategoriaContabilId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaContabilAtividades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriaContabilAtividades_CategoriasContabeis_CategoriaContabilId",
                        column: x => x.CategoriaContabilId,
                        principalTable: "CategoriasContabeis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DemandaFormHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    FormValuesId = table.Column<int>(nullable: false),
                    Revisao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFormHistoricos_DemandaFormValues_FormValuesId",
                        column: x => x.FormValuesId,
                        principalTable: "DemandaFormValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FaseTipoDetalhado",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FaseCadeiaProdutoId = table.Column<string>(nullable: false),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaseTipoDetalhado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaseTipoDetalhado_FasesCadeiaProduto_FaseCadeiaProdutoId",
                        column: x => x.FaseCadeiaProdutoId,
                        principalTable: "FasesCadeiaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoArquivos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AcessoFornecedor = table.Column<bool>(nullable: false),
                    CaptacaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoArquivos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Captacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    DemandaId = table.Column<int>(nullable: false),
                    EspecificacaoTecnicaFileId = table.Column<int>(nullable: false),
                    CriadorId = table.Column<string>(nullable: true),
                    UsuarioSuprimentoId = table.Column<string>(nullable: true),
                    ContratoSugeridoId = table.Column<int>(nullable: true),
                    ContratoId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EnvioCaptacao = table.Column<DateTime>(nullable: true),
                    Termino = table.Column<DateTime>(nullable: true),
                    Cancelamento = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Observacoes = table.Column<string>(nullable: true),
                    Consideracoes = table.Column<string>(nullable: true),
                    TemaId = table.Column<int>(nullable: true),
                    TemaOutro = table.Column<string>(nullable: true),
                    UsuarioRefinamentoId = table.Column<string>(nullable: true),
                    DataAlvo = table.Column<DateTime>(nullable: true),
                    ArquivoComprobatorioId = table.Column<int>(nullable: true),
                    PropostaSelecionadaId = table.Column<int>(nullable: true),
                    UsuarioAprovacaoId = table.Column<string>(nullable: true),
                    ArquivoRiscosId = table.Column<int>(nullable: true),
                    IsProjetoAprovado = table.Column<bool>(nullable: true),
                    ArquivoFormalizacaoId = table.Column<int>(nullable: true),
                    UsuarioExecucaoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captacoes_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Captacoes_Contratos_ContratoSugeridoId",
                        column: x => x.ContratoSugeridoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Captacoes_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoSubTemas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Outro = table.Column<string>(nullable: true),
                    CaptacaoId = table.Column<int>(nullable: false),
                    SubTemaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoSubTemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaptacaoSubTemas_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoSubTemas_Temas_SubTemaId",
                        column: x => x.SubTemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoComentarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemandaComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandaId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaComentarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DemandaLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Tela = table.Column<string>(nullable: true),
                    Acao = table.Column<int>(nullable: false),
                    StatusAnterior = table.Column<string>(nullable: true),
                    StatusNovo = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DemandaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demandas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    CriadorId = table.Column<string>(nullable: true),
                    SuperiorDiretoId = table.Column<string>(nullable: true),
                    RevisorId = table.Column<string>(nullable: true),
                    EtapaAtual = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CaptacaoDate = table.Column<DateTime>(nullable: true),
                    EspecificacaoTecnicaFileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demandas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UF = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false),
                    Categoria = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ResponsavelId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    NomeCompleto = table.Column<string>(nullable: true),
                    Cargo = table.Column<string>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    UltimoLogin = table.Column<DateTime>(nullable: true),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoSugestoesFornecedores",
                columns: table => new
                {
                    FornecedorId = table.Column<int>(nullable: false),
                    CaptacaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoSugestoesFornecedores", x => new { x.FornecedorId, x.CaptacaoId });
                    table.ForeignKey(
                        name: "FK_CaptacaoSugestoesFornecedores_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacaoSugestoesFornecedores_Empresas_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaptacoesFornecedores",
                columns: table => new
                {
                    CaptacaoId = table.Column<int>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacoesFornecedores", x => new { x.FornecedorId, x.CaptacaoId });
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaptacoesFornecedores_Empresas_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    DemandaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Demandas_DemandaId",
                        column: x => x.DemandaId,
                        principalTable: "Demandas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Files_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContratoComentarioFile",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContratoComentarioFile", x => new { x.ComentarioId, x.FileId });
                    table.ForeignKey(
                        name: "FK_ContratoComentarioFile_ContratoComentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "ContratoComentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContratoComentarioFile_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemandaFormFiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemandaFormId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    DemandaFormValuesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandaFormFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemandaFormFiles_DemandaFormValues_DemandaFormValuesId",
                        column: x => x.DemandaFormValuesId,
                        principalTable: "DemandaFormValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DemandaFormFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanoComentarios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    AuthorId = table.Column<string>(nullable: true),
                    Mensagem = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoComentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanoComentarios_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlanoComentarioFile",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanoComentarioFile", x => new { x.ComentarioId, x.FileId });
                    table.ForeignKey(
                        name: "FK_PlanoComentarioFile_PlanoComentarios_ComentarioId",
                        column: x => x.ComentarioId,
                        principalTable: "PlanoComentarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanoComentarioFile_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(nullable: true),
                    TituloCompleto = table.Column<string>(nullable: true),
                    Compartilhamento = table.Column<string>(nullable: false),
                    PlanoTrabalhoFileId = table.Column<int>(nullable: false),
                    ContratoId = table.Column<int>(nullable: false),
                    EspecificacaoTecnicaFileId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TemaId = table.Column<int>(nullable: true),
                    TemaOutro = table.Column<string>(nullable: true),
                    SegmentoId = table.Column<string>(nullable: true, defaultValue: "G"),
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
                        name: "FK_Projetos_AspNetUsers_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projetos_Segmentos_SegmentoId",
                        column: x => x.SegmentoId,
                        principalTable: "Segmentos",
                        principalColumn: "Valor",
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
                name: "ProjetoEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    EmpresaRefId = table.Column<int>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    Codigo = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Funcao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoEmpresas_Empresas_EmpresaRefId",
                        column: x => x.EmpresaRefId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetoEmpresas_Projetos_ProjetoId",
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
                        principalColumn: "Id");
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
                name: "ProjetosRelatoriosApoios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    CnpjReceptora = table.Column<string>(nullable: true),
                    Laboratorio = table.Column<string>(maxLength: 100, nullable: true),
                    LaboratorioArea = table.Column<string>(maxLength: 50, nullable: true),
                    MateriaisEquipamentos = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosApoios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosApoios_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosFinais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    IsProdutoAlcancado = table.Column<bool>(nullable: false),
                    TecnicaProduto = table.Column<string>(maxLength: 1000, nullable: true),
                    IsTecnicaImplementada = table.Column<bool>(nullable: false),
                    TecnicaImplementada = table.Column<string>(maxLength: 1000, nullable: true),
                    IsAplicabilidadeAlcancada = table.Column<bool>(nullable: false),
                    AplicabilidadeJustificativa = table.Column<string>(maxLength: 1000, nullable: true),
                    ResultadosTestes = table.Column<string>(maxLength: 1000, nullable: true),
                    AbrangenciaProduto = table.Column<string>(maxLength: 1000, nullable: true),
                    AmbitoAplicacaoProduto = table.Column<string>(maxLength: 1000, nullable: true),
                    TransferenciaTecnologica = table.Column<string>(maxLength: 500, nullable: true),
                    RelatorioArquivoId = table.Column<int>(nullable: true),
                    AuditoriaRelatorioArquivoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosFinais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosFinais_Files_AuditoriaRelatorioArquivoId",
                        column: x => x.AuditoriaRelatorioArquivoId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosFinais_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosFinais_Files_RelatorioArquivoId",
                        column: x => x.RelatorioArquivoId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosIndicadoresEconomicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    Beneficio = table.Column<string>(maxLength: 400, nullable: true),
                    UnidadeBase = table.Column<string>(maxLength: 10, nullable: true),
                    ValorNumerico = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentagemImpacto = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ValorBeneficio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosIndicadoresEconomicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosIndicadoresEconomicos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosProducoesCientificas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    DataPublicacao = table.Column<DateTime>(nullable: false),
                    ConfirmacaoPublicacao = table.Column<bool>(nullable: false),
                    NomeEventoPublicacao = table.Column<string>(maxLength: 50, nullable: true),
                    LinkPublicacao = table.Column<string>(maxLength: 50, nullable: true),
                    PaisId = table.Column<int>(nullable: false),
                    Cidade = table.Column<string>(maxLength: 30, nullable: true),
                    TituloTrabalho = table.Column<string>(maxLength: 200, nullable: true),
                    ArquivoTrabalhoOrigemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosProducoesCientificas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosProducoesCientificas_Files_ArquivoTrabalhoOrigemId",
                        column: x => x.ArquivoTrabalhoOrigemId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosProducoesCientificas_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosProducoesCientificas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    PedidoData = table.Column<DateTime>(nullable: false),
                    PedidoNumero = table.Column<string>(maxLength: 15, nullable: true),
                    TituloINPI = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosPropriedadesIntelectuais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosPropriedadesIntelectuais_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosSocioambiental",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    ResultadoPositivo = table.Column<bool>(nullable: false),
                    DescricaoResultado = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosSocioambiental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosSocioambiental_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosSubtemas",
                columns: table => new
                {
                    ProjetoId = table.Column<int>(nullable: false),
                    SubTemaId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Outro = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosSubtemas", x => new { x.ProjetoId, x.SubTemaId });
                    table.ForeignKey(
                        name: "FK_ProjetosSubtemas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosSubtemas_Temas_SubTemaId",
                        column: x => x.SubTemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetoXml",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    FileId = table.Column<int>(nullable: false),
                    Versao = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoXml", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoXml_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetoXml_Projetos_ProjetoId",
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
                    EmpresaId = table.Column<int>(nullable: false),
                    Documento = table.Column<string>(nullable: true),
                    ValorHora = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    UrlCurriculo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetoRecursosHumanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanos_ProjetoEmpresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "ProjetoEmpresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetoRecursosHumanos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id");
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
                name: "PropriedadeIntelectualDepositante",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropriedadeId = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: true),
                    Porcentagem = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropriedadeIntelectualDepositante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropriedadeIntelectualDepositante_ProjetoEmpresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "ProjetoEmpresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropriedadeIntelectualDepositante_ProjetosRelatoriosPropriedadesIntelectuais_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "ProjetosRelatoriosPropriedadesIntelectuais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosCapacitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    RecursoId = table.Column<int>(nullable: false),
                    Tipo = table.Column<string>(nullable: false),
                    IsConcluido = table.Column<bool>(nullable: false),
                    DataConclusao = table.Column<DateTime>(nullable: true),
                    CnpjInstituicao = table.Column<string>(maxLength: 20, nullable: true),
                    AreaPesquisa = table.Column<string>(maxLength: 50, nullable: true),
                    TituloTrabalhoOrigem = table.Column<string>(maxLength: 200, nullable: true),
                    ArquivoTrabalhoOrigemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosCapacitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosCapacitacoes_Files_ArquivoTrabalhoOrigemId",
                        column: x => x.ArquivoTrabalhoOrigemId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosCapacitacoes_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosCapacitacoes_ProjetoRecursosHumanos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "ProjetoRecursosHumanos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuaisInventores",
                columns: table => new
                {
                    PropriedadeId = table.Column<int>(nullable: false),
                    RecursoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosPropriedadesIntelectuaisInventores", x => new { x.PropriedadeId, x.RecursoId });
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosPropriedadesIntelectuaisInventores_ProjetosRelatoriosPropriedadesIntelectuais_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "ProjetosRelatoriosPropriedadesIntelectuais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosPropriedadesIntelectuaisInventores_ProjetoRecursosHumanos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "ProjetoRecursosHumanos",
                        principalColumn: "Id");
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
                    EmpresaFinanciadoraId = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRecursosAlocacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoEmpresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "ProjetoEmpresas",
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
                        name: "FK_ProjetosRecursosAlocacoes_ProjetoEmpresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "ProjetoEmpresas",
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
                    AuthorId = table.Column<string>(nullable: true),
                    Tipo = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    FinanciadoraId = table.Column<int>(nullable: false),
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
                    RecebedoraId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRegistrosFinanceiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_AspNetUsers_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoEmpresas_FinanciadoraId",
                        column: x => x.FinanciadoraId,
                        principalTable: "ProjetoEmpresas",
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
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoEmpresas_RecebedoraId",
                        column: x => x.RecebedoraId,
                        principalTable: "ProjetoEmpresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRegistrosFinanceiros_ProjetoRecursosMateriais_RecursoMaterialId",
                        column: x => x.RecursoMaterialId,
                        principalTable: "ProjetoRecursosMateriais",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProjetosRelatoriosEtapas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    AtividadesRealizadas = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjetosRelatoriosEtapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosEtapas_ProjetoEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "ProjetoEtapas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProjetosRelatoriosEtapas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "PropostaContratosRevisao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    ParentId = table.Column<int>(nullable: false),
                    Conteudo = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaContratosRevisao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaContratosRevisao_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Finalizado = table.Column<bool>(nullable: false),
                    Participacao = table.Column<int>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataAlteracao = table.Column<DateTime>(nullable: false),
                    DataResposta = table.Column<DateTime>(nullable: true),
                    DataParticipacao = table.Column<DateTime>(nullable: true),
                    DataClausulasAceitas = table.Column<DateTime>(nullable: true),
                    RelatorioId = table.Column<int>(nullable: true),
                    Duracao = table.Column<short>(nullable: false),
                    FornecedorId = table.Column<int>(nullable: false),
                    ResponsavelId = table.Column<string>(nullable: true),
                    CaptacaoId = table.Column<int>(nullable: false),
                    PlanoTrabalhoAprovacao = table.Column<int>(nullable: false),
                    ContratoAprovacao = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propostas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Propostas_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Propostas_Empresas_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Propostas_AspNetUsers_ResponsavelId",
                        column: x => x.ResponsavelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropostaContratos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Rascunho = table.Column<string>(nullable: true),
                    Conteudo = table.Column<string>(nullable: true),
                    Finalizado = table.Column<bool>(nullable: false),
                    FileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaContratos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaContratos_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "PropostaEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Required = table.Column<bool>(nullable: false),
                    EmpresaRefId = table.Column<int>(nullable: true),
                    CNPJ = table.Column<string>(nullable: true),
                    UF = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    Codigo = table.Column<string>(nullable: true),
                    Funcao = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEmpresas_Empresas_EmpresaRefId",
                        column: x => x.EmpresaRefId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropostaEmpresas_Propostas_PropostaId",
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
                    table.PrimaryKey("PK_PropostaEscopos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEscopos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropostaMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Objetivo = table.Column<string>(nullable: true),
                    Meses = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaMetas_Propostas_PropostaId",
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
                    MetodologiaTrabalho = table.Column<string>(nullable: true),
                    BuscaAnterioridade = table.Column<string>(nullable: true),
                    Bibliografia = table.Column<string>(nullable: true),
                    PesquisasCorrelatasPeDAneel = table.Column<string>(nullable: true),
                    PesquisasCorrelatasPeD = table.Column<string>(nullable: true),
                    PesquisasCorrelatasExecutora = table.Column<string>(nullable: true)
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
                    Titulo = table.Column<string>(nullable: true),
                    Descricao = table.Column<string>(nullable: true),
                    TipoId = table.Column<string>(nullable: true),
                    FaseCadeiaId = table.Column<string>(nullable: true),
                    TipoDetalhadoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaProdutos_FasesCadeiaProduto_FaseCadeiaId",
                        column: x => x.FaseCadeiaId,
                        principalTable: "FasesCadeiaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropostaProdutos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaProdutos_FaseTipoDetalhado_TipoDetalhadoId",
                        column: x => x.TipoDetalhadoId,
                        principalTable: "FaseTipoDetalhado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostaProdutos_ProdutoTipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "ProdutoTipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropostaRelatorios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DataAlteracao = table.Column<DateTime>(nullable: false),
                    Validacao = table.Column<string>(nullable: true),
                    FileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRelatorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRelatorios_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropostaRelatorios_Propostas_PropostaId",
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
                name: "PropostasArquivos",
                columns: table => new
                {
                    PropostaId = table.Column<int>(nullable: false),
                    ArquivoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostasArquivos", x => new { x.PropostaId, x.ArquivoId });
                    table.ForeignKey(
                        name: "FK_PropostasArquivos_Files_ArquivoId",
                        column: x => x.ArquivoId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropostasArquivos_Propostas_PropostaId",
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
                    EmpresaId = table.Column<int>(nullable: false),
                    Documento = table.Column<string>(nullable: true),
                    ValorHora = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
                    UrlCurriculo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosHumanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanos_PropostaEmpresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "PropostaEmpresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanos_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropostaEtapas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    DescricaoAtividades = table.Column<string>(nullable: true),
                    ProdutoId = table.Column<int>(nullable: false),
                    Meses = table.Column<string>(nullable: true),
                    Ordem = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaEtapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaEtapas_PropostaProdutos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "PropostaProdutos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaEtapas_Propostas_PropostaId",
                        column: x => x.PropostaId,
                        principalTable: "Propostas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "PropostaRecursosHumanosAlocacao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropostaId = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: false),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true),
                    RecursoId = table.Column<int>(nullable: false),
                    HoraMeses = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosHumanosAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosHumanosAlocacao_PropostaEmpresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "PropostaEmpresas",
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
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropostaRecursosMateriaisAlocacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEmpresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "PropostaEmpresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEmpresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "PropostaEmpresas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PropostaRecursosMateriaisAlocacao_PropostaEtapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "PropostaEtapas",
                        principalColumn: "Id");
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

            migrationBuilder.InsertData(
                table: "CategoriasContabeis",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, "Recursos Humanos", "RH" },
                    { 2, "Serviços de Terceiros", "ST" },
                    { 3, "Materiais de Consumo", "MC" },
                    { 4, "Viagens e Diárias", "VD" },
                    { 5, "CITENEL", "CT" },
                    { 6, "Auditoria Contábil e Financeira", "AC" },
                    { 7, "Outros", "OU" }
                });

            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Ativo", "Categoria", "Cnpj", "Discriminator", "Nome", "UF", "Valor" },
                values: new object[,]
                {
                    { 20, true, 1, "26.712.591/0001-13", "Empresa", "PARAGUAÇÚ", null, "11104" },
                    { 19, true, 1, "26.707.830/0001-47", "Empresa", "AIMORÉS", null, "11105" },
                    { 18, true, 1, "26.617.923/0001-80", "Empresa", "JANAÚBA", null, "11114" },
                    { 1, true, 1, "07.859.971/0001-30", "Empresa", "TAESA", null, "07130" },
                    { 17, true, 1, "24.944.194/0001-41", "Empresa", "MIRACEMA", null, "10731" },
                    { 2, true, 1, "07.859.971/0001-30", "Empresa", "ATE", null, "04906" },
                    { 3, true, 1, "07.859.971/0001-30", "Empresa", "ATE II", null, "05012" },
                    { 4, true, 1, "07.002.685/0001-54", "Empresa", "ATE III", null, "05455" },
                    { 5, true, 1, "07.859.971/0001-30", "Empresa", "ETEO", null, "	0414" },
                    { 8, true, 1, "07.859.971/0001-30", "Empresa", "MUNIRAH", null, "04757" },
                    { 7, true, 1, "19.486.977/0001-99", "Empresa", "MARIANA", null, "08837" },
                    { 21, true, 1, "28.052.123/0001-95", "Empresa", "ERB 1", null, "00000" },
                    { 9, true, 1, "07.859.971/0001-30", "Empresa", "NOVATRANS", null, "02609" },
                    { 10, true, 1, "07.859.971/0001-30", "Empresa", "NTE", null, "03619" },
                    { 11, true, 1, "07.859.971/0001-30", "Empresa", "PATESA", null, "03943" },
                    { 12, true, 1, "15.867.360/0001-62", "Empresa", "São Gotardo", null, "08193" },
                    { 13, true, 1, "07.859.971/0001-30", "Empresa", "STE", null, "03944" },
                    { 14, true, 1, "07.859.971/0001-30", "Empresa", "TSN", null, "02607" },
                    { 6, true, 1, "07.859.971/0001-30", "Empresa", "GTESA", null, "03624" },
                    { 15, true, 1, "05.063.249/0001-60", "Empresa", "ETAU", null, "03942" },
                    { 16, true, 1, "09.274.998/0001-97", "Empresa", "BRASNORTE", null, "06625" }
                });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 22, "RONDONIA", "RO" },
                    { 1, "ACRE", "AC" },
                    { 2, "ALAGOAS", "AL" },
                    { 3, "AMAPÁ", "AP" },
                    { 4, "AMAZONAS", "AM" },
                    { 5, "BAHIA", "BA" },
                    { 6, "CEARÁ", "CE" },
                    { 7, "DISTRITO FEDERAL", "DF" },
                    { 8, "ESPÍRITO SANTO", "ES" },
                    { 9, "GOIÁS", "GO" },
                    { 10, "MARANHÃO", "MA" },
                    { 11, "MATO GROSSO", "MT" },
                    { 20, "RIO GRANDE DO NORTE", "RN" },
                    { 12, "MATO GROSSO DO SUL", "MS" },
                    { 14, "PARÁ", "PA" },
                    { 15, "PARAÍBA", "PB" },
                    { 16, "PARANÁ", "PR" },
                    { 17, "PERNAMBUCO", "PE" },
                    { 18, "PIAUÍ", "PI" },
                    { 19, "RIO DE JANEIRO", "RJ" },
                    { 27, "TOCANTINS", "TO" },
                    { 26, "SERGIPE", "SE" },
                    { 25, "SÃO PAULO", "SP" },
                    { 24, "SANTA CATARINA", "SC" },
                    { 23, "RORAIMA", "RR" },
                    { 13, "MINAS GERAIS", "MG" },
                    { 21, "RIO GRANDE DO SUL", "RS" }
                });

            migrationBuilder.InsertData(
                table: "FasesCadeiaProduto",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { "CS", "Cabeça de série" },
                    { "LP", "Lote Pioneiro" },
                    { "DE", "Desenvolvimento Experimental" },
                    { "PA", "Pesquisa Aplicada" },
                    { "PB", "Pesquisa Básica Dirigida" },
                    { "IM", "Inserção no Mercado" }
                });

            migrationBuilder.InsertData(
                table: "ItemAjuda",
                columns: new[] { "Id", "Codigo", "Conteudo", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 13, "cadastro-fornecedor-ativo", null, "cadastro-fornecedor-ativo", "cadastro-fornecedor-ativo" },
                    { 12, "cadastro-co-executor-uf", null, "cadastro-co-executor-uf", "cadastro-co-executor-uf" },
                    { 11, "cadastro-co-executor-razao-social", null, "cadastro-co-executor-razao-social", "cadastro-co-executor-razao-social" },
                    { 10, "cadastro-co-executor-funcao", null, "cadastro-co-executor-funcao", "cadastro-co-executor-funcao" },
                    { 9, "cadastro-co-executor-cnpj", null, "cadastro-co-executor-cnpj", "cadastro-co-executor-cnpj" },
                    { 8, "alocacao-recurso-material-quantidade", null, "alocacao-recurso-material-quantidade", "alocacao-recurso-material-quantidade" },
                    { 5, "alocacao-recurso-material-empresa-financiadora", null, "alocacao-recurso-material-empresa-financiadora", "alocacao-recurso-material-empresa-financiadora" },
                    { 6, "alocacao-recurso-material-empresa-recebedora", null, "alocacao-recurso-material-empresa-recebedora", "alocacao-recurso-material-empresa-recebedora" },
                    { 4, "alocacao-recurso-material", null, "alocacao-recurso-material", "alocacao-recurso-material" },
                    { 3, "alocacao-recurso-humano-financiadora", null, "alocacao-recurso-humano-financiadora", "alocacao-recurso-humano-financiadora" },
                    { 2, "alocacao-recurso-humano-etapa", null, "alocacao-recurso-humano-etapa", "alocacao-recurso-humano-etapa" },
                    { 1, "alocacao-recurso-humano", null, "alocacao-recurso-humano", "alocacao-recurso-humano" },
                    { 14, "cadastro-fornecedor-cnpj", null, "cadastro-fornecedor-cnpj", "cadastro-fornecedor-cnpj" },
                    { 7, "alocacao-recurso-material-etapa", null, "alocacao-recurso-material-etapa", "alocacao-recurso-material-etapa" },
                    { 15, "cadastro-fornecedor-email-responsavel", null, "cadastro-fornecedor-email-responsavel", "cadastro-fornecedor-email-responsavel" },
                    { 35, "meu-cadastro-email", null, "meu-cadastro-email", "meu-cadastro-email" },
                    { 17, "cadastro-fornecedor-nome-responsavel", null, "cadastro-fornecedor-nome-responsavel", "cadastro-fornecedor-nome-responsavel" },
                    { 38, "proposta-config-data-maxima", null, "proposta-config-data-maxima", "proposta-config-data-maxima" },
                    { 39, "proposta-config-fornecedores", null, "proposta-config-fornecedores", "proposta-config-fornecedores" },
                    { 40, "recurso-humano-brasileiro", null, "recurso-humano-brasileiro", "recurso-humano-brasileiro" },
                    { 41, "recurso-humano-cpf", null, "recurso-humano-cpf", "recurso-humano-cpf" },
                    { 42, "recurso-humano-curriculo-lattes", null, "recurso-humano-curriculo-lattes", "recurso-humano-curriculo-lattes" },
                    { 43, "recurso-humano-empresa", null, "recurso-humano-empresa", "recurso-humano-empresa" },
                    { 37, "proposta-config-consideracoes", null, "proposta-config-consideracoes", "proposta-config-consideracoes" },
                    { 44, "recurso-humano-funcao", null, "recurso-humano-funcao", "recurso-humano-funcao" },
                    { 46, "recurso-humano-titulacao", null, "recurso-humano-titulacao", "recurso-humano-titulacao" },
                    { 47, "recurso-humano-valor-hora", null, "recurso-humano-valor-hora", "recurso-humano-valor-hora" },
                    { 48, "recurso-material-categoria-contabil", null, "recurso-material-categoria-contabil", "recurso-material-categoria-contabil" },
                    { 49, "recurso-material-nome", null, "recurso-material-nome", "recurso-material-nome" },
                    { 50, "recurso-material-valor-unitario", null, "recurso-material-valor-unitario", "recurso-material-valor-unitario" },
                    { 51, "risco-classificacao", null, "risco-classificacao", "risco-classificacao" },
                    { 45, "recurso-humano-nome-completo", null, "recurso-humano-nome-completo", "recurso-humano-nome-completo" },
                    { 36, "meu-cadastro-nome-completo", null, "meu-cadastro-nome-completo", "meu-cadastro-nome-completo" },
                    { 34, "demanda-superior-direto", null, "demanda-superior-direto", "demanda-superior-direto" },
                    { 33, "demanda-status", null, "demanda-status", "demanda-status" },
                    { 18, "cadastro-produto-cadeia-inovacao", null, "cadastro-produto-cadeia-inovacao", "cadastro-produto-cadeia-inovacao" },
                    { 19, "cadastro-produto-tipo-detalhado", null, "cadastro-produto-tipo-detalhado", "cadastro-produto-tipo-detalhado" },
                    { 20, "cadastro-produto-tipo-produto", null, "cadastro-produto-tipo-produto", "cadastro-produto-tipo-produto" },
                    { 21, "cadastro-produto-titulo", null, "cadastro-produto-titulo", "cadastro-produto-titulo" },
                    { 22, "cadastro-usuario-email", null, "cadastro-usuario-email", "cadastro-usuario-email" },
                    { 23, "cadastro-usuario-nome-completo", null, "cadastro-usuario-nome-completo", "cadastro-usuario-nome-completo" },
                    { 24, "cadastro-usuario-tipo", null, "cadastro-usuario-tipo", "cadastro-usuario-tipo" },
                    { 25, "captacao-consideracoes", null, "captacao-consideracoes", "captacao-consideracoes" },
                    { 26, "captacao-contrato", null, "captacao-contrato", "captacao-contrato" },
                    { 27, "captacao-data-maxima", null, "captacao-data-maxima", "captacao-data-maxima" },
                    { 28, "captacao-data-maxima-ext", null, "captacao-data-maxima-ext", "captacao-data-maxima-ext" },
                    { 29, "captacao-data-proposta", null, "captacao-data-proposta", "captacao-data-proposta" },
                    { 30, "captacao-equipe", null, "captacao-equipe", "captacao-equipe" },
                    { 31, "captacao-observacoes", null, "captacao-observacoes", "captacao-observacoes" },
                    { 32, "contrato-titulo", null, "contrato-titulo", "contrato-titulo" },
                    { 16, "cadastro-fornecedor-nome", null, "cadastro-fornecedor-nome", "cadastro-fornecedor-nome" },
                    { 52, "risco-item", null, "risco-item", "risco-item" },
                    { 53, "risco-probabilidade", null, "risco-probabilidade", "risco-probabilidade" }
                });

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 75, "Fiji" },
                    { 239, "Territórios Austrais Franceses" },
                    { 240, "Timor Leste" },
                    { 241, "Togo" },
                    { 242, "Tokelau" },
                    { 243, "Tonga" },
                    { 244, "Trindade e Tobago" },
                    { 245, "Tunísia" },
                    { 246, "Turquemenistão" },
                    { 247, "Turquia" },
                    { 248, "Tuvalu" },
                    { 249, "Ucrânia" },
                    { 250, "Uganda" },
                    { 251, "Uruguai" },
                    { 238, "Território Britânico do Oceano Índico" },
                    { 252, "Uzbequistão" },
                    { 254, "Vaticano" },
                    { 255, "Venezuela" },
                    { 256, "Vietnã" },
                    { 257, "Wallis e Futuna" },
                    { 258, "Zâmbia" },
                    { 259, "Zimbabué" },
                    { 72, "Estônia" },
                    { 71, "Estados Unidos" },
                    { 70, "Espanha" },
                    { 69, "Eslovênia" },
                    { 68, "Eslováquia" },
                    { 67, "Escócia" },
                    { 66, "Eritreia" },
                    { 253, "Vanuatu" },
                    { 237, "Tanzânia" },
                    { 236, "Tajiquistão" },
                    { 235, "Taiwan" },
                    { 206, "República Dominicana" },
                    { 207, "Romênia" },
                    { 208, "Ruanda" },
                    { 209, "Rússia" },
                    { 210, "Saara Ocidental" },
                    { 211, "Samoa" },
                    { 212, "Samoa Americana" },
                    { 213, "Santa Helena" },
                    { 214, "Santa Lúcia" },
                    { 74, "Faroé" },
                    { 216, "São Marinho" },
                    { 217, "São Pedro e Miquelon" },
                    { 218, "São Tomé e Príncipe" },
                    { 219, "São Vicente e Granadinas" },
                    { 220, "Seicheles" },
                    { 221, "Senegal" },
                    { 222, "Serra Leoa" },
                    { 223, "Sérvia" },
                    { 224, "Singapura" },
                    { 225, "Síria" },
                    { 226, "Somália" },
                    { 227, "Sri Lanka" },
                    { 228, "Suazilândia" },
                    { 229, "Sudão" },
                    { 230, "Sudão do Sul" },
                    { 231, "Suécia" },
                    { 232, "Suíça" },
                    { 233, "Suriname" },
                    { 234, "Tailândia" },
                    { 65, "Equador" },
                    { 205, "República do Congo" },
                    { 64, "Emirados Árabes Unidos" },
                    { 62, "Egito" },
                    { 28, "Bermudas" },
                    { 27, "Benim" },
                    { 26, "Belize" },
                    { 25, "Bélgica" },
                    { 24, "Barém" },
                    { 23, "Barbados" },
                    { 22, "Bangladesh" },
                    { 21, "Bahamas" },
                    { 20, "Azerbaijão" },
                    { 19, "Áustria" },
                    { 18, "Austrália" },
                    { 17, "Aruba" },
                    { 16, "Armênia" },
                    { 29, "Bielorrússia" },
                    { 15, "Argentina" },
                    { 13, "Arábia Saudita" },
                    { 12, "Antilhas Holandesas" },
                    { 11, "Antígua e Barbuda" },
                    { 10, "Antártida" },
                    { 9, "Anguila" },
                    { 8, "Angola" },
                    { 7, "Andorra" },
                    { 6, "Alemanha" },
                    { 5, "Albânia" },
                    { 4, "África do Sul" },
                    { 3, "Afeganistão" },
                    { 2, "Acrotiri e Deceleia" },
                    { 1, "Açores" },
                    { 14, "Argélia" },
                    { 30, "Bolívia" },
                    { 31, "Bósnia e Herzegovina" },
                    { 32, "Botsuana" },
                    { 61, "Domínica" },
                    { 60, "Djibuti" },
                    { 59, "Dinamarca" },
                    { 58, "Curaçao" },
                    { 57, "Cuba" },
                    { 56, "Croácia" },
                    { 55, "Costa Rica" },
                    { 54, "Costa do Marfim" },
                    { 53, "Coreia do Sul" },
                    { 52, "Coreia do Norte" },
                    { 51, "Comores" },
                    { 50, "Colômbia" },
                    { 49, "Chipre" },
                    { 48, "China" },
                    { 47, "Chile" },
                    { 46, "Chade" },
                    { 45, "Cazaquistão" },
                    { 44, "Catar" },
                    { 43, "Canárias" },
                    { 42, "Canadá" },
                    { 41, "Camboja" },
                    { 40, "Camarões" },
                    { 39, "Cabo Verde" },
                    { 38, "Butão" },
                    { 37, "Burundi" },
                    { 36, "Burquina Faso" },
                    { 35, "Bulgária" },
                    { 34, "Brunei" },
                    { 33, "Brasil" },
                    { 63, "El Salvador" },
                    { 204, "República Democrática do Congo" },
                    { 215, "São Cristóvão e Neves" },
                    { 202, "República Centro-Africana" },
                    { 127, "Índia" },
                    { 126, "Ilhas Virgens Britânicas" },
                    { 125, "Ilhas Virgens Americanas" },
                    { 124, "Ilhas Turcas e Caicos" },
                    { 123, "Ilhas Spratly" },
                    { 122, "Ilhas Salomão" },
                    { 128, "Indonésia" },
                    { 121, "Ilhas Pitcairn" },
                    { 119, "Ilhas Marshall" },
                    { 118, "Ilhas Heard e McDonald" },
                    { 117, "Ilhas Falkland" },
                    { 116, "Ilhas do mar de coral" },
                    { 115, "Ilhas Cook" },
                    { 114, "Ilhas Cocos" },
                    { 120, "Ilhas Paracel" },
                    { 129, "Inglaterra" },
                    { 130, "Irã" },
                    { 131, "Iraque" },
                    { 146, "Líbano" },
                    { 145, "Letônia" },
                    { 144, "Lesoto" },
                    { 143, "Laos" },
                    { 142, "Kuwait" },
                    { 141, "Kosovo" },
                    { 140, "Jordânia" },
                    { 139, "Jersey" },
                    { 138, "Japão" },
                    { 137, "Jamaica" },
                    { 136, "Itália" },
                    { 135, "Israel" },
                    { 134, "Islândia" },
                    { 133, "Irlanda do norte" },
                    { 132, "Irlanda" },
                    { 113, "Ilhas Caimão" },
                    { 112, "Ilhas Ashmore e Cartier" },
                    { 111, "Ilha Wake" },
                    { 110, "Ilha Norfolk" },
                    { 90, "Guatemala" },
                    { 89, "Guam" },
                    { 88, "Guadalupe" },
                    { 87, "Groenlândia" },
                    { 86, "Grécia" },
                    { 85, "Granada" },
                    { 84, "Gibraltar" },
                    { 83, "Geórgia do Sul e Sandwich do Sul" },
                    { 82, "Geórgia" },
                    { 81, "Gana" },
                    { 80, "Gâmbia" },
                    { 79, "Gabão" },
                    { 78, "França" },
                    { 77, "Finlândia" },
                    { 76, "Filipinas" },
                    { 91, "Guernsey" },
                    { 147, "Libéria" },
                    { 92, "Guiana" },
                    { 94, "Guiné" },
                    { 109, "Ilha Jan Mayen" },
                    { 108, "Ilha do Natal" },
                    { 107, "Ilha de Navassa" },
                    { 106, "Ilha de Man" },
                    { 105, "Ilha de Clipperton" },
                    { 104, "Ilha da Madeira" },
                    { 103, "Ilha Bouvet" },
                    { 102, "Iêmen" },
                    { 101, "Hungria" },
                    { 100, "Hong Kong" },
                    { 99, "Honduras" },
                    { 98, "Holanda (Países baixos)" },
                    { 97, "Haiti" },
                    { 96, "Guiné Equatorial" },
                    { 95, "Guiné Bissau" },
                    { 93, "Guiana Francesa" },
                    { 203, "República Checa" },
                    { 158, "Mali" },
                    { 165, "Mayotte" },
                    { 162, "Martinica" },
                    { 163, "Maurício" },
                    { 164, "Mauritânia" },
                    { 192, "Paraguai" },
                    { 166, "México" },
                    { 167, "Micronésia" },
                    { 168, "Moçambique" },
                    { 169, "Moldávia" },
                    { 170, "Mônaco" },
                    { 171, "Mongólia" },
                    { 172, "Monserrate" },
                    { 173, "Montenegro" },
                    { 174, "Myanmar" },
                    { 161, "Marrocos" },
                    { 175, "Namíbia" },
                    { 177, "Nepal" },
                    { 178, "Nicarágua" },
                    { 179, "Níger" },
                    { 180, "Nigéria" },
                    { 181, "Niue" },
                    { 182, "Noruega" },
                    { 183, "Nova Caledônia" },
                    { 184, "Nova Zelândia" },
                    { 185, "Omã" },
                    { 186, "País de Gales" },
                    { 187, "Palau" },
                    { 188, "Palestina" },
                    { 189, "Panamá" },
                    { 176, "Nauru" },
                    { 160, "Marianas do Norte" },
                    { 159, "Malta" },
                    { 73, "Etiópia" },
                    { 201, "Reino Unido" },
                    { 200, "Quiribati" },
                    { 199, "Quirguizistão" },
                    { 198, "Quênia" },
                    { 197, "Portugal" },
                    { 196, "Porto Rico" },
                    { 195, "Polônia" },
                    { 194, "Polinésia Francesa" },
                    { 193, "Peru" },
                    { 190, "Papua-Nova Guiné" },
                    { 191, "Paquistão" },
                    { 149, "Liechtenstein" },
                    { 157, "Maldivas" },
                    { 156, "Malawi" },
                    { 148, "Líbia" },
                    { 155, "Malásia" },
                    { 154, "Madagascar" },
                    { 153, "Macedônia" },
                    { 152, "Macau" },
                    { 151, "Luxemburgo" },
                    { 150, "Lituânia" }
                });

            migrationBuilder.InsertData(
                table: "ProdutoTipos",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { "CM", "Conceito ou Metodologia" },
                    { "SW", "Software" },
                    { "MS", "Material ou Substância" },
                    { "ME", "Máquina ou Equipamento" },
                    { "CD", "Componente ou Dispositivo" },
                    { "SM", "Sistema" }
                });

            migrationBuilder.InsertData(
                table: "Segmentos",
                columns: new[] { "Valor", "Nome" },
                values: new object[,]
                {
                    { "C", "Comercialização" },
                    { "D", "Distribuição" },
                    { "T", "Transmissão" },
                    { "G", "Geração" }
                });

            migrationBuilder.InsertData(
                table: "Temas",
                columns: new[] { "Id", "Nome", "Order", "ParentId", "Valor" },
                values: new object[,]
                {
                    { 60, "Supervisão, Controle e Proteção de Sistemas de Energia Elétrica", 0, null, "SC" },
                    { 1, "Fontes alternativas de geração de energia elétrica", 0, null, "FA" },
                    { 7, "Geração Termelétrica", 0, null, "GT" },
                    { 14, "Gestão de Bacias e Reservatórios", 0, null, "GB" },
                    { 22, "Meio Ambiente", 0, null, "MA" },
                    { 27, "Segurança", 0, null, "SE" },
                    { 33, "Eficiência Energética", 0, null, "EE" },
                    { 39, "Planejamento de Sistemas de Energia Elétrica", 0, null, "PL" },
                    { 48, "Operação de Sistemas de Energia Elétrica", 0, null, "OP" },
                    { 92, "Outros", 1, null, "OU" },
                    { 72, "Qualidade e Confiabilidade dos Serviços de Energia Elétrica", 0, null, "QC" },
                    { 80, "Medição, faturamento e combate a perdas comerciais", 0, null, "MF" }
                });

            migrationBuilder.InsertData(
                table: "CategoriaContabilAtividades",
                columns: new[] { "Id", "CategoriaContabilId", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, 1, "Dedicação horária dos membros da equipe de gestão do Programa de P&D da Empresa, quadro efetivo.", "HH" },
                    { 13, 7, "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI).", "BA" },
                    { 11, 7, "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação.", "EC" },
                    { 10, 6, "Contratação de auditoria contábil e financeira para os projetos concluídos.", "CA" },
                    { 9, 5, "Apoio à realização do CITENEL.", "AC" },
                    { 8, 4, "Participação dos responsáveis técnicos pelos projetos de P&D nas avaliações presenciais convocadas pela ANEEL.", "AP" },
                    { 12, 7, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" },
                    { 6, 4, "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D.", "PP" },
                    { 5, 4, "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação.", "EC" },
                    { 4, 3, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" },
                    { 3, 2, "Prospecção tecnológica e demais atividades necessárias ao planejamento e à elaboração do plano estratégico de investimento em P&D.", "PP" },
                    { 2, 2, "Desenvolvimento de ferramenta para gestão do Programa de P&D da Empresa, excluindose aquisição de equipamentos.", "FG" },
                    { 7, 4, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" }
                });

            migrationBuilder.InsertData(
                table: "FaseTipoDetalhado",
                columns: new[] { "Id", "FaseCadeiaProdutoId", "Nome", "Valor" },
                values: new object[,]
                {
                    { 16, "LP", "Primeira fabricação de produto", "" },
                    { 17, "LP", "Reprodução de licenças para ensaios de validação", "" },
                    { 18, "LP", "Análise de custos e refino do projeto, com vistas à produção industrial e/ou à comercialização", "" },
                    { 19, "IM", "Estudos mercadológicos", "" },
                    { 22, "IM", "Contratação de empresa de transferência de tecnologia e serviços jurídicos", "" },
                    { 21, "IM", "Registro de patentes", "" },
                    { 23, "IM", "Aprimoramentos e melhorias incrementais nos produtos", "" },
                    { 24, "IM", "Software ou serviços", "" },
                    { 14, "DE", "Software baseado em pesquisa aplicada", "" },
                    { 20, "IM", "Material de divulgação", "" },
                    { 13, "DE", "Serviços (novos ou aperfeiçoados)", "" },
                    { 15, "CS", "Aperfeiçoamento de protótipo obtido em projeto anterior", "" },
                    { 11, "DE", "Protótipo de equipamento para demonstração e testes", "" },
                    { 12, "DE", "Implantação de projeto piloto", "" },
                    { 1, "PB", "Novo material", "" },
                    { 2, "PB", "Nova estrutura", "" },
                    { 4, "PB", "Algoritmo", "" },
                    { 5, "PA", "metodologia ou técnica", "" },
                    { 3, "PB", "Modelo", "" },
                    { 7, "PA", "Modelos digitais", "" },
                    { 8, "PA", "Modelos de funções ou de processos", "" },
                    { 9, "DE", "Protótipo de material para demonstração e testes", "" },
                    { 10, "DE", "Protótipo de dispositivo para demonstração e testes", "" },
                    { 6, "PA", "Projeto demonstrativo de novos equipamentos", "" }
                });

            migrationBuilder.InsertData(
                table: "Temas",
                columns: new[] { "Id", "Nome", "Order", "ParentId", "Valor" },
                values: new object[,]
                {
                    { 61, "Implementação de sistemas de controle (robusto, adaptativo e inteligente).", 0, 60, "SC01" },
                    { 67, "Desenvolvimento e aplicação de sistemas de medição fasorial.", 0, 60, "SC07" },
                    { 66, "Novas tecnologias para supervisão do fornecimento de energia elétrica.", 0, 60, "SC06" },
                    { 65, "Técnicas de inteligência artificial aplicadas ao controle, operação e proteção de sistemas elétricos.", 0, 60, "SC05" },
                    { 64, "Desenvolvimento de técnicas para recomposição de sistemas elétricos.", 0, 60, "SC04" },
                    { 63, "Técnicas eficientes de restauração rápida de grandes centros de carga.", 0, 60, "SC03" },
                    { 62, "Análise dinâmica de sistemas em tempo real.", 0, 60, "SC02" },
                    { 59, "Outros.", 0, 48, "OP0X" },
                    { 53, "Alocação de fontes de potência reativa em sistemas de distribuição.", 0, 48, "OP05" },
                    { 57, "Desenvolvimento e/ou aprimoramento dos modelos de previsão de chuva versus vazão.", 0, 48, "OP09" },
                    { 56, "Desenvolvimento de modelos para a otimização de despacho hidrotérmico.", 0, 48, "OP08" },
                    { 55, "Análise das grandes perturbações e impactos no planejamento, operação e controle.", 0, 48, "OP07" },
                    { 54, "Estudo, simulação e análise do desempenho de sistemas elétricos de potência.", 0, 48, "OP06" },
                    { 52, "Otimização estrutural e paramétrica da capacidade dos sistemas de distribuição.", 0, 48, "OP04" },
                    { 51, "Estruturas, funções e regras de operação dos mercados de serviços ancilares.", 0, 48, "OP03" },
                    { 68, "Análise de falhas em sistemas elétricos.", 0, 60, "SC08" },
                    { 58, "Sistemas de monitoramento da operação de usinas não-despachadas pelo ONS.", 0, 48, "OP10" },
                    { 69, "Compatibilidade eletromagnética em sistemas elétricos.", 0, 60, "SC09" },
                    { 90, "Sistemas de tarifação e novas estruturas tarifárias.", 0, 80, "MF10" },
                    { 71, "Outro.", 1, 60, "SC0X" },
                    { 50, "Critérios de gerenciamento de carga para diferentes níveis de hierarquia.", 0, 48, "OP02" },
                    { 89, "Sistemas centralizados de medição, controle e gerenciamento de energia em consumidores finais.", 0, 80, "MF09" },
                    { 88, "Impacto dos projetos de eficiência energética na redução de perdas comerciais.", 0, 80, "MF08" },
                    { 87, "Gerenciamento dos equipamentos de medição (qualidade e redução de falhas).", 0, 80, "MF07" },
                    { 86, "Uso de indicadores socioeconômicos, dados fiscais e gastos com outros insumos.", 0, 80, "MF06" },
                    { 85, "Energia economizada e agregada ao mercado após regularização de fraudes.", 0, 80, "MF05" },
                    { 84, "Diagnóstico, prospecção e redução da vulnerabilidade de sistemas elétricos ao furto e à fraude.", 0, 80, "MF04" },
                    { 83, "Desenvolvimento de tecnologias para combate à fraude e ao furto de energia elétrica.", 0, 80, "MF03" },
                    { 82, "Estimação, análise e redução de perdas técnicas em sistemas elétricos.", 0, 80, "MF02" },
                    { 81, "Avaliação econômica para definição da perda mínima atingível.", 0, 80, "MF01" },
                    { 79, "Outro.", 1, 72, "QC0X" },
                    { 78, "Compensação financeira por violação de indicadores de qualidade.", 0, 72, "QC06" },
                    { 77, "Impactos econômicos e aspectos contratuais da qualidade da energia elétrica.", 0, 72, "QC05" },
                    { 76, "Curvas de sensibilidade e de suportabilidade de equipamentos.", 0, 72, "QC04" },
                    { 75, "Requisitos para conexão de cargas potencialmente perturbadoras no sistema elétrico.", 0, 72, "QC03" },
                    { 74, "Modelagem e análise dos distúrbios associados à qualidade da energia elétrica.", 0, 72, "QC02" },
                    { 73, "Sistemas e técnicas de monitoração e gerenciamento da qualidade da energia elétrica.", 0, 72, "QC01" },
                    { 70, "Sistemas de aterramento.", 0, 60, "SC10" },
                    { 49, "Ferramentas de apoio à operação de sistemas elétricos de potência em tempo real.", 0, 48, "OP01" },
                    { 26, "Outro.", 1, 22, "MA0X" },
                    { 46, "Tecnologias e sistemas de transmissão de energia em longas distâncias.", 0, 39, "PL07" },
                    { 20, "Assoreamento de reservatórios formados por barragens de usinas hidrelétricas.", 0, 14, "GB06" },
                    { 19, "Gestão da segurança de barragens de usinas hidrelétricas.", 0, 14, "GB05" },
                    { 18, "Gestão sócio-patrimonial de reservatórios de usinas hidrelétricas.", 0, 14, "GB04" },
                    { 17, "Integração e otimização do uso múltiplo de reservatórios hidrelétricos.", 0, 14, "GB03" },
                    { 16, "Efeitos de mudanças climáticas globais no regime hidrológico de bacias hidrográficas.", 0, 14, "GB02" },
                    { 15, "Emissões de gases de efeito estufa (GEE) em reservatórios de usinas hidrelétricas.", 0, 14, "GB01" },
                    { 13, "Outro.", 1, 7, "GT0X" },
                    { 12, "Técnicas para captura e seqüestro de carbono de termelétricas.", 0, 7, "GT05" },
                    { 11, "Micro-sistemas de cogeração residenciais.", 0, 7, "GT04" },
                    { 10, "Otimização da geração de energia elétrica em plantas industriais: aumento de eficiência na cogeração.", 0, 7, "GT03" },
                    { 9, "Novas técnicas para eficientização e diminuição da emissão de poluentes de usinas termelétricas a combustível derivado de petróleo.", 0, 7, "GT02" },
                    { 8, "Avaliação de riscos e incertezas do fornecimento contínuo de gás natural para geração termelétrica.", 0, 7, "GT01" },
                    { 6, "Outro.", 1, 1, "FA0X" },
                    { 5, "Tecnologias para aproveitamento de novos combustíveis em plantas geradoras.", 0, 1, "FA04" },
                    { 4, "Novos materiais e equipamentos para geração de energia por fontes alternativas.", 0, 1, "FA03" },
                    { 3, "Geração de energia a partir de resíduos sólidos urbanos.", 0, 1, "FA02" },
                    { 2, "Alternativas energéticas sustentáveis de atendimento a pequenos sistemas isolados.", 0, 1, "FA01" },
                    { 21, "Outro.", 1, 14, "GB0X" },
                    { 47, "Outro.", 1, 39, "PL0X" },
                    { 23, "Impactos e restrições socioambientais de sistemas de energia elétrica.", 0, 22, "MA01" },
                    { 25, "Estudos de toxicidade relacionados à deterioração da qualidade da água em reservatórios. ", 0, 22, "MA03" },
                    { 45, "Materiais supercondutores para transmissão de energia elétrica.", 0, 39, "PL06" },
                    { 44, "Modelos hidrodinâmicos aplicados em reservatórios de usinas hidrelétricas.", 0, 39, "PL05" },
                    { 43, "Metodologia de previsão de mercado para diferentes níveis temporais e estratégias de contratação.", 0, 39, "PL04" },
                    { 42, "Integração de geração distribuída a redes elétricas.", 0, 39, "PL03" },
                    { 41, "Integração de centrais eólicas ao SIN.", 0, 39, "PL02" },
                    { 40, "Planejamento integrado da expansão de sistemas elétricos.", 0, 39, "PL01" },
                    { 38, "Outro.", 1, 33, "EE0X" },
                    { 37, "Metodologias para avaliação de resultados de projetos de eficiência energética.", 0, 33, "EE04" },
                    { 36, "Definição de indicadores de eficiência energética.", 0, 33, "EE03" },
                    { 35, "Gerenciamento de carga pelo lado da demanda.", 0, 33, "EE02" },
                    { 34, "Novas tecnologias para melhoria da eficiência energética.", 0, 33, "EE01" },
                    { 32, "Outro.", 1, 27, "SE0X" },
                    { 31, "Novas tecnologias para inspeção e manutenção de sistemas elétricos.", 0, 27, "SE04" },
                    { 30, "Novas tecnologias para equipamentos de proteção individual.", 0, 27, "SE03" },
                    { 29, "Análise e mitigação de riscos de acidentes elétricos.", 0, 27, "SE02" },
                    { 28, "Identificação e mitigação dos impactos de campos eletromagnéticos em organismos vivos.", 0, 27, "SE01" },
                    { 91, "Outro.", 1, 80, "MF0X" },
                    { 24, "Metodologias para mensuração econômico-financeira de externalidades em sistemas de energia elétrica.", 0, 22, "MA02" },
                    { 93, "Outros", 0, 92, "OU  " }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoArquivos_CaptacaoId",
                table: "CaptacaoArquivos",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoArquivos_UserId",
                table: "CaptacaoArquivos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSubTemas_CaptacaoId",
                table: "CaptacaoSubTemas",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSubTemas_SubTemaId",
                table: "CaptacaoSubTemas",
                column: "SubTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSugestoesFornecedores_CaptacaoId",
                table: "CaptacaoSugestoesFornecedores",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoComprobatorioId",
                table: "Captacoes",
                column: "ArquivoComprobatorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoFormalizacaoId",
                table: "Captacoes",
                column: "ArquivoFormalizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ArquivoRiscosId",
                table: "Captacoes",
                column: "ArquivoRiscosId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ContratoId",
                table: "Captacoes",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_ContratoSugeridoId",
                table: "Captacoes",
                column: "ContratoSugeridoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_CriadorId",
                table: "Captacoes",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_DemandaId",
                table: "Captacoes",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_EspecificacaoTecnicaFileId",
                table: "Captacoes",
                column: "EspecificacaoTecnicaFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_PropostaSelecionadaId",
                table: "Captacoes",
                column: "PropostaSelecionadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_TemaId",
                table: "Captacoes",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioAprovacaoId",
                table: "Captacoes",
                column: "UsuarioAprovacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioExecucaoId",
                table: "Captacoes",
                column: "UsuarioExecucaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioRefinamentoId",
                table: "Captacoes",
                column: "UsuarioRefinamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Captacoes_UsuarioSuprimentoId",
                table: "Captacoes",
                column: "UsuarioSuprimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacoesFornecedores_CaptacaoId",
                table: "CaptacoesFornecedores",
                column: "CaptacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaContabilAtividades_CategoriaContabilId",
                table: "CategoriaContabilAtividades",
                column: "CategoriaContabilId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarioFile_FileId",
                table: "ContratoComentarioFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarios_AuthorId",
                table: "ContratoComentarios",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContratoComentarios_PropostaId",
                table: "ContratoComentarios",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaComentarios_DemandaId",
                table: "DemandaComentarios",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaComentarios_UserId",
                table: "DemandaComentarios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormFiles_DemandaFormValuesId",
                table: "DemandaFormFiles",
                column: "DemandaFormValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormFiles_FileId",
                table: "DemandaFormFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaFormHistoricos_FormValuesId",
                table: "DemandaFormHistoricos",
                column: "FormValuesId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaLogs_DemandaId",
                table: "DemandaLogs",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_DemandaLogs_UserId",
                table: "DemandaLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_CriadorId",
                table: "Demandas",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_EspecificacaoTecnicaFileId",
                table: "Demandas",
                column: "EspecificacaoTecnicaFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_RevisorId",
                table: "Demandas",
                column: "RevisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Demandas_SuperiorDiretoId",
                table: "Demandas",
                column: "SuperiorDiretoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_ResponsavelId",
                table: "Empresas",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_FaseTipoDetalhado_FaseCadeiaProdutoId",
                table: "FaseTipoDetalhado",
                column: "FaseCadeiaProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_DemandaId",
                table: "Files",
                column: "DemandaId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UserId",
                table: "Files",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAjuda_Codigo",
                table: "ItemAjuda",
                column: "Codigo",
                unique: true,
                filter: "[Codigo] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarioFile_FileId",
                table: "PlanoComentarioFile",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarios_AuthorId",
                table: "PlanoComentarios",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanoComentarios_PropostaId",
                table: "PlanoComentarios",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoArquivos_ArquivoId",
                table: "ProjetoArquivos",
                column: "ArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEmpresas_EmpresaRefId",
                table: "ProjetoEmpresas",
                column: "EmpresaRefId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoEmpresas_ProjetoId",
                table: "ProjetoEmpresas",
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
                unique: true);

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
                name: "IX_Projetos_SegmentoId",
                table: "Projetos",
                column: "SegmentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_TemaId",
                table: "Projetos",
                column: "TemaId");

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
                name: "IX_ProjetosRecursosAlocacoes_EmpresaRecebedoraId",
                table: "ProjetosRecursosAlocacoes",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRecursosAlocacoes_RecursoMaterialId",
                table: "ProjetosRecursosAlocacoes",
                column: "RecursoMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRegistrosFinanceiros_AuthorId",
                table: "ProjetosRegistrosFinanceiros",
                column: "AuthorId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosApoios_ProjetoId",
                table: "ProjetosRelatoriosApoios",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosCapacitacoes_ArquivoTrabalhoOrigemId",
                table: "ProjetosRelatoriosCapacitacoes",
                column: "ArquivoTrabalhoOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosCapacitacoes_ProjetoId",
                table: "ProjetosRelatoriosCapacitacoes",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosCapacitacoes_RecursoId",
                table: "ProjetosRelatoriosCapacitacoes",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosEtapas_EtapaId",
                table: "ProjetosRelatoriosEtapas",
                column: "EtapaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosEtapas_ProjetoId",
                table: "ProjetosRelatoriosEtapas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosFinais_AuditoriaRelatorioArquivoId",
                table: "ProjetosRelatoriosFinais",
                column: "AuditoriaRelatorioArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosFinais_ProjetoId",
                table: "ProjetosRelatoriosFinais",
                column: "ProjetoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosFinais_RelatorioArquivoId",
                table: "ProjetosRelatoriosFinais",
                column: "RelatorioArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosIndicadoresEconomicos_ProjetoId",
                table: "ProjetosRelatoriosIndicadoresEconomicos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosProducoesCientificas_ArquivoTrabalhoOrigemId",
                table: "ProjetosRelatoriosProducoesCientificas",
                column: "ArquivoTrabalhoOrigemId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosProducoesCientificas_PaisId",
                table: "ProjetosRelatoriosProducoesCientificas",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosProducoesCientificas_ProjetoId",
                table: "ProjetosRelatoriosProducoesCientificas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosPropriedadesIntelectuais_ProjetoId",
                table: "ProjetosRelatoriosPropriedadesIntelectuais",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosPropriedadesIntelectuaisInventores_RecursoId",
                table: "ProjetosRelatoriosPropriedadesIntelectuaisInventores",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosRelatoriosSocioambiental_ProjetoId",
                table: "ProjetosRelatoriosSocioambiental",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetosSubtemas_SubTemaId",
                table: "ProjetosSubtemas",
                column: "SubTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoXml_FileId",
                table: "ProjetoXml",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjetoXml_ProjetoId",
                table: "ProjetoXml",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_FileId",
                table: "PropostaContratos",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_ParentId",
                table: "PropostaContratos",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratos_PropostaId",
                table: "PropostaContratos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratosRevisao_ParentId",
                table: "PropostaContratosRevisao",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratosRevisao_PropostaId",
                table: "PropostaContratosRevisao",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaContratosRevisao_UserId",
                table: "PropostaContratosRevisao",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEmpresas_EmpresaRefId",
                table: "PropostaEmpresas",
                column: "EmpresaRefId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEmpresas_PropostaId",
                table: "PropostaEmpresas",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEscopos_PropostaId",
                table: "PropostaEscopos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaEtapas_ProdutoId",
                table: "PropostaEtapas",
                column: "ProdutoId");

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
                name: "IX_PropostaMetas_PropostaId",
                table: "PropostaMetas",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaPlanosTrabalhos_PropostaId",
                table: "PropostaPlanosTrabalhos",
                column: "PropostaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_FaseCadeiaId",
                table: "PropostaProdutos",
                column: "FaseCadeiaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_PropostaId",
                table: "PropostaProdutos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_TipoDetalhadoId",
                table: "PropostaProdutos",
                column: "TipoDetalhadoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaProdutos_TipoId",
                table: "PropostaProdutos",
                column: "TipoId");

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
                name: "IX_PropostaRelatorios_FileId",
                table: "PropostaRelatorios",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRelatorios_PropostaId",
                table: "PropostaRelatorios",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropostaRiscos_PropostaId",
                table: "PropostaRiscos",
                column: "PropostaId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_FornecedorId",
                table: "Propostas",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_RelatorioId",
                table: "Propostas",
                column: "RelatorioId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_ResponsavelId",
                table: "Propostas",
                column: "ResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_CaptacaoId_FornecedorId",
                table: "Propostas",
                columns: new[] { "CaptacaoId", "FornecedorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropostasArquivos_ArquivoId",
                table: "PropostasArquivos",
                column: "ArquivoId");

            migrationBuilder.CreateIndex(
                name: "IX_PropriedadeIntelectualDepositante_EmpresaId",
                table: "PropriedadeIntelectualDepositante",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropriedadeIntelectualDepositante_PropriedadeId",
                table: "PropriedadeIntelectualDepositante",
                column: "PropriedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_ParentId",
                table: "Temas",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaptacaoArquivos_AspNetUsers_UserId",
                table: "CaptacaoArquivos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaptacaoArquivos_Captacoes_CaptacaoId",
                table: "CaptacaoArquivos",
                column: "CaptacaoId",
                principalTable: "Captacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_CriadorId",
                table: "Captacoes",
                column: "CriadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioAprovacaoId",
                table: "Captacoes",
                column: "UsuarioAprovacaoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioExecucaoId",
                table: "Captacoes",
                column: "UsuarioExecucaoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioRefinamentoId",
                table: "Captacoes",
                column: "UsuarioRefinamentoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioSuprimentoId",
                table: "Captacoes",
                column: "UsuarioSuprimentoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoComprobatorioId",
                table: "Captacoes",
                column: "ArquivoComprobatorioId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoFormalizacaoId",
                table: "Captacoes",
                column: "ArquivoFormalizacaoId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_ArquivoRiscosId",
                table: "Captacoes",
                column: "ArquivoRiscosId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Files_EspecificacaoTecnicaFileId",
                table: "Captacoes",
                column: "EspecificacaoTecnicaFileId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Demandas_DemandaId",
                table: "Captacoes",
                column: "DemandaId",
                principalTable: "Demandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Propostas_PropostaSelecionadaId",
                table: "Captacoes",
                column: "PropostaSelecionadaId",
                principalTable: "Propostas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoComentarios_AspNetUsers_AuthorId",
                table: "ContratoComentarios",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContratoComentarios_Propostas_PropostaId",
                table: "ContratoComentarios",
                column: "PropostaId",
                principalTable: "Propostas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandaComentarios_AspNetUsers_UserId",
                table: "DemandaComentarios",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandaComentarios_Demandas_DemandaId",
                table: "DemandaComentarios",
                column: "DemandaId",
                principalTable: "Demandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandaLogs_AspNetUsers_UserId",
                table: "DemandaLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DemandaLogs_Demandas_DemandaId",
                table: "DemandaLogs",
                column: "DemandaId",
                principalTable: "Demandas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_AspNetUsers_CriadorId",
                table: "Demandas",
                column: "CriadorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_AspNetUsers_RevisorId",
                table: "Demandas",
                column: "RevisorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_AspNetUsers_SuperiorDiretoId",
                table: "Demandas",
                column: "SuperiorDiretoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Demandas_Files_EspecificacaoTecnicaFileId",
                table: "Demandas",
                column: "EspecificacaoTecnicaFileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_AspNetUsers_ResponsavelId",
                table: "Empresas",
                column: "ResponsavelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanoComentarios_Propostas_PropostaId",
                table: "PlanoComentarios",
                column: "PropostaId",
                principalTable: "Propostas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Propostas_PropostaId",
                table: "Projetos",
                column: "PropostaId",
                principalTable: "Propostas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaContratosRevisao_Propostas_PropostaId",
                table: "PropostaContratosRevisao",
                column: "PropostaId",
                principalTable: "Propostas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PropostaContratosRevisao_PropostaContratos_ParentId",
                table: "PropostaContratosRevisao",
                column: "ParentId",
                principalTable: "PropostaContratos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Propostas_PropostaRelatorios_RelatorioId",
                table: "Propostas",
                column: "RelatorioId",
                principalTable: "PropostaRelatorios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_CriadorId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioAprovacaoId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioExecucaoId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioRefinamentoId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Captacoes_AspNetUsers_UsuarioSuprimentoId",
                table: "Captacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_AspNetUsers_CriadorId",
                table: "Demandas");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_AspNetUsers_RevisorId",
                table: "Demandas");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_AspNetUsers_SuperiorDiretoId",
                table: "Demandas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_AspNetUsers_ResponsavelId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_AspNetUsers_UserId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_AspNetUsers_ResponsavelId",
                table: "Propostas");

            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_Empresas_FornecedorId",
                table: "Propostas");

            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_Captacoes_CaptacaoId",
                table: "Propostas");

            migrationBuilder.DropForeignKey(
                name: "FK_Demandas_Files_EspecificacaoTecnicaFileId",
                table: "Demandas");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRelatorios_Files_FileId",
                table: "PropostaRelatorios");

            migrationBuilder.DropForeignKey(
                name: "FK_PropostaRelatorios_Propostas_PropostaId",
                table: "PropostaRelatorios");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CaptacaoArquivos");

            migrationBuilder.DropTable(
                name: "CaptacaoSubTemas");

            migrationBuilder.DropTable(
                name: "CaptacaoSugestoesFornecedores");

            migrationBuilder.DropTable(
                name: "CaptacoesFornecedores");

            migrationBuilder.DropTable(
                name: "CategoriaContabilAtividades");

            migrationBuilder.DropTable(
                name: "Clausulas");

            migrationBuilder.DropTable(
                name: "ContratoComentarioFile");

            migrationBuilder.DropTable(
                name: "DemandaComentarios");

            migrationBuilder.DropTable(
                name: "DemandaFormFiles");

            migrationBuilder.DropTable(
                name: "DemandaFormHistoricos");

            migrationBuilder.DropTable(
                name: "DemandaLogs");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "ItemAjuda");

            migrationBuilder.DropTable(
                name: "PlanoComentarioFile");

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
                name: "ProjetosRelatoriosApoios");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosCapacitacoes");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosEtapas");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosFinais");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosIndicadoresEconomicos");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosProducoesCientificas");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuaisInventores");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosSocioambiental");

            migrationBuilder.DropTable(
                name: "ProjetosSubtemas");

            migrationBuilder.DropTable(
                name: "ProjetoXml");

            migrationBuilder.DropTable(
                name: "PropostaContratosRevisao");

            migrationBuilder.DropTable(
                name: "PropostaEscopos");

            migrationBuilder.DropTable(
                name: "PropostaEtapasProdutos");

            migrationBuilder.DropTable(
                name: "PropostaMetas");

            migrationBuilder.DropTable(
                name: "PropostaPlanosTrabalhos");

            migrationBuilder.DropTable(
                name: "PropostaRecursosHumanosAlocacao");

            migrationBuilder.DropTable(
                name: "PropostaRecursosMateriaisAlocacao");

            migrationBuilder.DropTable(
                name: "PropostaRiscos");

            migrationBuilder.DropTable(
                name: "PropostasArquivos");

            migrationBuilder.DropTable(
                name: "PropriedadeIntelectualDepositante");

            migrationBuilder.DropTable(
                name: "SystemOptions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ContratoComentarios");

            migrationBuilder.DropTable(
                name: "DemandaFormValues");

            migrationBuilder.DropTable(
                name: "PlanoComentarios");

            migrationBuilder.DropTable(
                name: "ProjetosRecursosAlocacoes");

            migrationBuilder.DropTable(
                name: "ProjetosRegistrosFinanceiros");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "PropostaContratos");

            migrationBuilder.DropTable(
                name: "PropostaRecursosHumanos");

            migrationBuilder.DropTable(
                name: "PropostaEtapas");

            migrationBuilder.DropTable(
                name: "PropostaRecursosMateriais");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuais");

            migrationBuilder.DropTable(
                name: "ProjetoEtapas");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosHumanos");

            migrationBuilder.DropTable(
                name: "ProjetoRecursosMateriais");

            migrationBuilder.DropTable(
                name: "PropostaEmpresas");

            migrationBuilder.DropTable(
                name: "PropostaProdutos");

            migrationBuilder.DropTable(
                name: "ProjetoProdutos");

            migrationBuilder.DropTable(
                name: "ProjetoEmpresas");

            migrationBuilder.DropTable(
                name: "CategoriasContabeis");

            migrationBuilder.DropTable(
                name: "FaseTipoDetalhado");

            migrationBuilder.DropTable(
                name: "ProdutoTipos");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropTable(
                name: "FasesCadeiaProduto");

            migrationBuilder.DropTable(
                name: "Segmentos");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Captacoes");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Temas");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Demandas");

            migrationBuilder.DropTable(
                name: "Propostas");

            migrationBuilder.DropTable(
                name: "PropostaRelatorios");
        }
    }
}
