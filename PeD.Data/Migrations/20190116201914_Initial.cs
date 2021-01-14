using System;
using Microsoft.EntityFrameworkCore.Metadata;
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
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CatalogEmpresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CatalogEmpresas", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CatalogSegmentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Segmento = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CatalogSegmentos", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CatalogStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CatalogStatus", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CatalogTema",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CatalogTema", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "CatalogUserPermissoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Valor = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_CatalogUserPermissoes", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
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
                    Status = table.Column<int>(nullable: false),
                    NomeCompleto = table.Column<string>(nullable: true),
                    CatalogEmpresaId = table.Column<int>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true),
                    FotoPerfil = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    UltimoLogin = table.Column<DateTime>(nullable: false),
                    DataCadastro = table.Column<DateTime>(nullable: false),
                    DataAtualizacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_CatalogEmpresas_CatalogEmpresaId",
                        column: x => x.CatalogEmpresaId,
                        principalTable: "CatalogEmpresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CatalogSubTemas",
                columns: table => new
                {
                    SubTemaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    CatalogTemaId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogSubTemas", x => x.SubTemaId);
                    table.ForeignKey(
                        name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                        column: x => x.CatalogTemaId,
                        principalTable: "CatalogTema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Temas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    CatalogTemaId = table.Column<int>(nullable: false),
                    OutroDesc = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temas_CatalogTema_CatalogTemaId",
                        column: x => x.CatalogTemaId,
                        principalTable: "CatalogTema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: false),
                    CatalogUserPermissaoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProjetos_CatalogUserPermissoes_CatalogUserPermissaoId",
                        column: x => x.CatalogUserPermissaoId,
                        principalTable: "CatalogUserPermissoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjetos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Titulo = table.Column<string>(nullable: true),
                    TituloDesc = table.Column<string>(nullable: true),
                    Numero = table.Column<string>(nullable: true),
                    CatalogEmpresaId = table.Column<int>(nullable: true),
                    CatalogStatusId = table.Column<int>(nullable: true),
                    CatalogSegmentoId = table.Column<int>(nullable: true),
                    AvaliacaoInicial = table.Column<string>(nullable: true),
                    CompartResultados = table.Column<string>(nullable: true),
                    Motivacao = table.Column<string>(nullable: true),
                    Originalidade = table.Column<string>(nullable: true),
                    Aplicabilidade = table.Column<string>(nullable: true),
                    Relevancia = table.Column<string>(nullable: true),
                    Razoabilidade = table.Column<string>(nullable: true),
                    Pesquisas = table.Column<string>(nullable: true),
                    TemaId = table.Column<int>(nullable: true),
                    TemaId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projetos_CatalogEmpresas_CatalogEmpresaId",
                        column: x => x.CatalogEmpresaId,
                        principalTable: "CatalogEmpresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projetos_CatalogSegmentos_CatalogSegmentoId",
                        column: x => x.CatalogSegmentoId,
                        principalTable: "CatalogSegmentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projetos_CatalogStatus_CatalogStatusId",
                        column: x => x.CatalogStatusId,
                        principalTable: "CatalogStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projetos_Temas_TemaId1",
                        column: x => x.TemaId1,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemaSubTemas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    CatalogTemaId = table.Column<int>(nullable: false),
                    CatalogSubTemaId = table.Column<int>(nullable: false),
                    OutroDesc = table.Column<string>(nullable: true),
                    TemaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemaSubTemas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemaSubTemas_CatalogSubTemas_CatalogSubTemaId",
                        column: x => x.CatalogSubTemaId,
                        principalTable: "CatalogSubTemas",
                        principalColumn: "SubTemaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemaSubTemas_CatalogTema_CatalogTemaId",
                        column: x => x.CatalogTemaId,
                        principalTable: "CatalogTema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemaSubTemas_Temas_TemaId",
                        column: x => x.TemaId,
                        principalTable: "Temas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Classificacao = table.Column<int>(nullable: false),
                    CatalogEmpresaId = table.Column<int>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    Uf = table.Column<string>(nullable: true),
                    RazaoSocial = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresas_CatalogEmpresas_CatalogEmpresaId",
                        column: x => x.CatalogEmpresaId,
                        principalTable: "CatalogEmpresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Empresas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etapas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    Desc = table.Column<string>(nullable: true),
                    DataInicio = table.Column<DateTime>(nullable: false),
                    DataFim = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etapas_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecursoMaterais",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: true),
                    Nome = table.Column<string>(nullable: true),
                    CategoriaContabil = table.Column<int>(nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Especificacao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursoMaterais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursoMaterais_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecursoHumanos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: false),
                    ValorHora = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    NomeCompleto = table.Column<string>(nullable: true),
                    Titulacao = table.Column<int>(nullable: false),
                    Funcao = table.Column<int>(nullable: false),
                    Nacionalidade = table.Column<int>(nullable: false),
                    CPF = table.Column<string>(nullable: true),
                    Passaporte = table.Column<string>(nullable: true),
                    UrlCurriculo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecursoHumanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecursoHumanos_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecursoHumanos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    ProjetoId = table.Column<int>(nullable: false),
                    Titulo = table.Column<string>(nullable: true),
                    Desc = table.Column<string>(nullable: true),
                    Classificacao = table.Column<int>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    FaseCadeia = table.Column<int>(nullable: false),
                    EtapaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produtos_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlocacoesRm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    EtapaId = table.Column<int>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: true),
                    RecursoMaterialId = table.Column<int>(nullable: true),
                    EmpresaFinanciadoraId = table.Column<int>(nullable: true),
                    EmpresaRecebedoraId = table.Column<int>(nullable: true),
                    Qtd = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlocacoesRm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlocacoesRm_Empresas_EmpresaFinanciadoraId",
                        column: x => x.EmpresaFinanciadoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRm_Empresas_EmpresaRecebedoraId",
                        column: x => x.EmpresaRecebedoraId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRm_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRm_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRm_RecursoMaterais_RecursoMaterialId",
                        column: x => x.RecursoMaterialId,
                        principalTable: "RecursoMaterais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AlocacoesRh",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    EtapaId = table.Column<int>(nullable: true),
                    ProjetoId = table.Column<int>(nullable: true),
                    RecursoHumanoId = table.Column<int>(nullable: true),
                    EmpresaId = table.Column<int>(nullable: true),
                    ValorHora = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    HrsMes1 = table.Column<int>(nullable: false),
                    HrsMes2 = table.Column<int>(nullable: false),
                    HrsMes3 = table.Column<int>(nullable: false),
                    HrsMes4 = table.Column<int>(nullable: false),
                    HrsMes5 = table.Column<int>(nullable: false),
                    HrsMes6 = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlocacoesRh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlocacoesRh_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRh_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRh_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AlocacoesRh_RecursoHumanos_RecursoHumanoId",
                        column: x => x.RecursoHumanoId,
                        principalTable: "RecursoHumanos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRh_EmpresaId",
                table: "AlocacoesRh",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRh_EtapaId",
                table: "AlocacoesRh",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRh_ProjetoId",
                table: "AlocacoesRh",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRh_RecursoHumanoId",
                table: "AlocacoesRh",
                column: "RecursoHumanoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRm_EmpresaFinanciadoraId",
                table: "AlocacoesRm",
                column: "EmpresaFinanciadoraId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRm_EmpresaRecebedoraId",
                table: "AlocacoesRm",
                column: "EmpresaRecebedoraId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRm_EtapaId",
                table: "AlocacoesRm",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRm_ProjetoId",
                table: "AlocacoesRm",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_AlocacoesRm_RecursoMaterialId",
                table: "AlocacoesRm",
                column: "RecursoMaterialId");

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
                name: "IX_AspNetUsers_CatalogEmpresaId",
                table: "AspNetUsers",
                column: "CatalogEmpresaId");

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
                name: "IX_CatalogSubTemas_CatalogTemaId",
                table: "CatalogSubTemas",
                column: "CatalogTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CatalogEmpresaId",
                table: "Empresas",
                column: "CatalogEmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_ProjetoId",
                table: "Empresas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Etapas_ProjetoId",
                table: "Etapas",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_EtapaId",
                table: "Produtos",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_ProjetoId",
                table: "Produtos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_CatalogEmpresaId",
                table: "Projetos",
                column: "CatalogEmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_CatalogSegmentoId",
                table: "Projetos",
                column: "CatalogSegmentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_CatalogStatusId",
                table: "Projetos",
                column: "CatalogStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_TemaId1",
                table: "Projetos",
                column: "TemaId1");

            migrationBuilder.CreateIndex(
                name: "IX_RecursoHumanos_EmpresaId",
                table: "RecursoHumanos",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursoHumanos_ProjetoId",
                table: "RecursoHumanos",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursoMaterais_ProjetoId",
                table: "RecursoMaterais",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_Temas_CatalogTemaId",
                table: "Temas",
                column: "CatalogTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_TemaSubTemas_CatalogSubTemaId",
                table: "TemaSubTemas",
                column: "CatalogSubTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_TemaSubTemas_CatalogTemaId",
                table: "TemaSubTemas",
                column: "CatalogTemaId");

            migrationBuilder.CreateIndex(
                name: "IX_TemaSubTemas_TemaId",
                table: "TemaSubTemas",
                column: "TemaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjetos_CatalogUserPermissaoId",
                table: "UserProjetos",
                column: "CatalogUserPermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjetos_UserId",
                table: "UserProjetos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlocacoesRh");

            migrationBuilder.DropTable(
                name: "AlocacoesRm");

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
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "TemaSubTemas");

            migrationBuilder.DropTable(
                name: "UserProjetos");

            migrationBuilder.DropTable(
                name: "RecursoHumanos");

            migrationBuilder.DropTable(
                name: "RecursoMaterais");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Etapas");

            migrationBuilder.DropTable(
                name: "CatalogSubTemas");

            migrationBuilder.DropTable(
                name: "CatalogUserPermissoes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Projetos");

            migrationBuilder.DropTable(
                name: "CatalogEmpresas");

            migrationBuilder.DropTable(
                name: "CatalogSegmentos");

            migrationBuilder.DropTable(
                name: "CatalogStatus");

            migrationBuilder.DropTable(
                name: "Temas");

            migrationBuilder.DropTable(
                name: "CatalogTema");
        }
    }
}