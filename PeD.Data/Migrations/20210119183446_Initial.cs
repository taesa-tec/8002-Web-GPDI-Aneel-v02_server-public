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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FasesCadeiaProduto", x => x.Id);
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
                name: "Segmentos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Segmentos", x => x.Id);
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
                    FaseCadeiaProdutoId = table.Column<int>(nullable: false),
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
                    CriadorId = table.Column<string>(nullable: true),
                    UsuarioSuprimentoId = table.Column<string>(nullable: true),
                    ContratoSugeridoId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    EnvioCaptacao = table.Column<DateTime>(nullable: true),
                    Termino = table.Column<DateTime>(nullable: true),
                    Cancelamento = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Observacoes = table.Column<string>(nullable: true),
                    Consideracoes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Captacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Captacoes_Contratos_ContratoSugeridoId",
                        column: x => x.ContratoSugeridoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CaptacaoContratos",
                columns: table => new
                {
                    CaptacaoId = table.Column<int>(nullable: false),
                    ContratoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaptacaoContratos", x => new { x.CaptacaoId, x.ContratoId });
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Captacoes_CaptacaoId",
                        column: x => x.CaptacaoId,
                        principalTable: "Captacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaptacaoContratos_Contratos_ContratoId",
                        column: x => x.ContratoId,
                        principalTable: "Contratos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    CaptacaoDate = table.Column<DateTime>(nullable: true)
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
                    FotoPerfil = table.Column<string>(nullable: true),
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
                name: "Propostas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FornecedorId = table.Column<int>(nullable: false),
                    CaptacaoId = table.Column<int>(nullable: false),
                    Finalizado = table.Column<bool>(nullable: false),
                    Participacao = table.Column<int>(nullable: false),
                    DataCriacao = table.Column<DateTime>(nullable: false),
                    DataResposta = table.Column<DateTime>(nullable: true),
                    DataClausulasAceitas = table.Column<DateTime>(nullable: true)
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

            migrationBuilder.InsertData(
                table: "CategoriasContabeis",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, "Recursos Humanos", "RH" },
                    { 2, "Serviços de Terceiros", "ST" },
                    { 3, "Materiais de Consumo", "MC" },
                    { 4, "Viagens e Diárias", "VD" },
                    { 5, "Outros", "OU" },
                    { 6, "CITENEL", "CT" },
                    { 7, "Auditoria Contábil e Financeira", "AC" }
                });

            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Ativo", "Categoria", "Cnpj", "Discriminator", "Nome", "Valor" },
                values: new object[,]
                {
                    { 3, true, 1, "07.859.971/0001-30", "Empresa", "ATE II", "05012" },
                    { 4, true, 1, "07.002.685/0001-54", "Empresa", "ATE III", "05455" },
                    { 5, true, 1, "07.859.971/0001-30", "Empresa", "ETEO", "	0414" },
                    { 6, true, 1, "07.859.971/0001-30", "Empresa", "GTESA", "03624" },
                    { 7, true, 1, "19.486.977/0001-99", "Empresa", "MARIANA", "08837" },
                    { 8, true, 1, "07.859.971/0001-30", "Empresa", "MUNIRAH", "04757" },
                    { 9, true, 1, "07.859.971/0001-30", "Empresa", "NOVATRANS", "02609" },
                    { 11, true, 1, "07.859.971/0001-30", "Empresa", "PATESA", "03943" },
                    { 12, true, 1, "15.867.360/0001-62", "Empresa", "São Gotardo", "08193" },
                    { 15, true, 1, "05.063.249/0001-60", "Empresa", "ETAU", "03942" },
                    { 14, true, 1, "07.859.971/0001-30", "Empresa", "TSN", "02607" },
                    { 2, true, 1, "07.859.971/0001-30", "Empresa", "ATE", "04906" },
                    { 16, true, 1, "09.274.998/0001-97", "Empresa", "BRASNORTE", "06625" },
                    { 17, true, 1, "24.944.194/0001-41", "Empresa", "MIRACEMA", "10731" },
                    { 18, true, 1, "26.617.923/0001-80", "Empresa", "JANAÚBA", "11114" },
                    { 19, true, 1, "26.707.830/0001-47", "Empresa", "AIMORÉS", "11105" },
                    { 20, true, 1, "26.712.591/0001-13", "Empresa", "PARAGUAÇÚ", "11104" },
                    { 21, true, 1, "28.052.123/0001-95", "Empresa", "ERB 1", "00000" },
                    { 13, true, 1, "07.859.971/0001-30", "Empresa", "STE", "03944" },
                    { 1, true, 1, "07.859.971/0001-30", "Empresa", "TAESA", "07130" },
                    { 10, true, 1, "07.859.971/0001-30", "Empresa", "NTE", "03619" }
                });

            migrationBuilder.InsertData(
                table: "Estados",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 3, "AMAPÁ", "AP" },
                    { 6, "CEARÁ", "CE" },
                    { 7, "DISTRITO FEDERAL", "DF" },
                    { 8, "ESPÍRITO SANTO", "ES" },
                    { 9, "GOIÁS", "GO" },
                    { 10, "MARANHÃO", "MA" },
                    { 11, "MATO GROSSO", "MT" },
                    { 12, "MATO GROSSO DO SUL", "MS" },
                    { 13, "MINAS GERAIS", "MG" },
                    { 14, "PARÁ", "PA" },
                    { 5, "BAHIA", "BA" },
                    { 15, "PARAÍBA", "PB" },
                    { 17, "PERNAMBUCO", "PE" },
                    { 18, "PIAUÍ", "PI" },
                    { 19, "RIO DE JANEIRO", "RJ" },
                    { 20, "RIO GRANDE DO NORTE", "RN" },
                    { 21, "RIO GRANDE DO SUL", "RS" },
                    { 22, "RONDONIA", "RO" },
                    { 23, "RORAIMA", "RR" },
                    { 24, "SANTA CATARINA", "SC" },
                    { 25, "SÃO PAULO", "SP" },
                    { 16, "PARANÁ", "PR" },
                    { 4, "AMAZONAS", "AM" },
                    { 27, "TOCANTINS", "TO" },
                    { 1, "ACRE", "AC" },
                    { 2, "ALAGOAS", "AL" },
                    { 26, "SERGIPE", "SE" }
                });

            migrationBuilder.InsertData(
                table: "FasesCadeiaProduto",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, "Pesquisa Básica Dirigida", "PB" },
                    { 2, "Pesquisa Aplicada", "PA" },
                    { 3, "Desenvolvimento Experimental", "DE" },
                    { 4, "Cabeça de série", "CS" },
                    { 5, "Lote Pioneiro", "LP" },
                    { 6, "Inserção no Mercado", "IM" }
                });

            migrationBuilder.InsertData(
                table: "Segmentos",
                columns: new[] { "Id", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, "Geração", "G" },
                    { 2, "Transmissão", "T" },
                    { 3, "Distribuição", "D" },
                    { 4, "Comercialização", "C" }
                });

            migrationBuilder.InsertData(
                table: "Temas",
                columns: new[] { "Id", "Nome", "Order", "ParentId", "Valor" },
                values: new object[,]
                {
                    { 60, "Supervisão, Controle e Proteção de Sistemas de Energia Elétrica", 0, null, "SC" },
                    { 80, "Medição, faturamento e combate a perdas comerciais", 0, null, "MF" },
                    { 48, "Operação de Sistemas de Energia Elétrica", 0, null, "OP" },
                    { 72, "Qualidade e Confiabilidade dos Serviços de Energia Elétrica", 0, null, "QC" },
                    { 39, "Planejamento de Sistemas de Energia Elétrica", 0, null, "PL" },
                    { 1, "Fontes alternativas de geração de energia elétrica", 0, null, "FA" },
                    { 27, "Segurança", 0, null, "SE" },
                    { 22, "Meio Ambiente", 0, null, "MA" },
                    { 14, "Gestão de Bacias e Reservatórios", 0, null, "GB" },
                    { 7, "Geração Termelétrica", 0, null, "GT" },
                    { 33, "Eficiência Energética", 0, null, "EE" },
                    { 92, "Outros", 1, null, "OU" }
                });

            migrationBuilder.InsertData(
                table: "CategoriaContabilAtividades",
                columns: new[] { "Id", "CategoriaContabilId", "Nome", "Valor" },
                values: new object[,]
                {
                    { 1, 1, "Dedicação horária dos membros da equipe de gestão do Programa de P&D da Empresa, quadro efetivo.", "HH" },
                    { 13, 7, "Contratação de auditoria contábil e financeira para os projetos concluídos.", "CA" },
                    { 11, 5, "Buscas de anterioridade no Instituto Nacional da Propriedade Industrial (INPI).", "BA" },
                    { 10, 5, "Divulgação de resultados de projetos de P&D, concluídos e/ou em execução.", "RP" },
                    { 9, 5, "Participação dos membros da equipe de gestão em eventos sobre pesquisa, desenvolvimento e inovação relacionados ao setor elétrico e/ou em cursos de gestão tecnológica e da informação.", "EC" },
                    { 8, 4, "Participação dos responsáveis técnicos pelos projetos de P&D nas avaliações presenciais convocadas pela ANEEL.", "AP" },
                    { 12, 6, "Apoio à realização do CITENEL.", "AC" },
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
                    { 16, 5, "Primeira fabricação de produto", "" },
                    { 17, 5, "Reprodução de licenças para ensaios de validação", "" },
                    { 18, 5, "Análise de custos e refino do projeto, com vistas à produção industrial e/ou à comercialização", "" },
                    { 19, 6, "Estudos mercadológicos", "" },
                    { 22, 6, "Contratação de empresa de transferência de tecnologia e serviços jurídicos", "" },
                    { 21, 6, "Registro de patentes", "" },
                    { 23, 6, "Aprimoramentos e melhorias incrementais nos produtos", "" },
                    { 24, 6, "Software ou serviços", "" },
                    { 14, 3, "Software baseado em pesquisa aplicada", "" },
                    { 20, 6, "Material de divulgação", "" },
                    { 13, 3, "Serviços (novos ou aperfeiçoados)", "" },
                    { 15, 4, "Aperfeiçoamento de protótipo obtido em projeto anterior", "" },
                    { 11, 3, "Protótipo de equipamento para demonstração e testes", "" },
                    { 12, 3, "Implantação de projeto piloto", "" },
                    { 1, 1, "Novo material", "" },
                    { 2, 1, "Nova estrutura", "" },
                    { 4, 1, "Algoritmo", "" },
                    { 5, 2, "metodologia ou técnica", "" },
                    { 3, 1, "Modelo", "" },
                    { 7, 2, "Modelos digitais", "" },
                    { 8, 2, "Modelos de funções ou de processos", "" },
                    { 9, 3, "Protótipo de material para demonstração e testes", "" },
                    { 10, 3, "Protótipo de dispositivo para demonstração e testes", "" },
                    { 6, 2, "Projeto demonstrativo de novos equipamentos", "" }
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
                name: "IX_CaptacaoContratos_ContratoId",
                table: "CaptacaoContratos",
                column: "ContratoId");

            migrationBuilder.CreateIndex(
                name: "IX_CaptacaoSugestoesFornecedores_CaptacaoId",
                table: "CaptacaoSugestoesFornecedores",
                column: "CaptacaoId");

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
                name: "IX_Propostas_FornecedorId",
                table: "Propostas",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Propostas_CaptacaoId_FornecedorId",
                table: "Propostas",
                columns: new[] { "CaptacaoId", "FornecedorId" },
                unique: true);

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
                name: "FK_Captacoes_AspNetUsers_UsuarioSuprimentoId",
                table: "Captacoes",
                column: "UsuarioSuprimentoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Captacoes_Demandas_DemandaId",
                table: "Captacoes",
                column: "DemandaId",
                principalTable: "Demandas",
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
                name: "FK_Empresas_AspNetUsers_ResponsavelId",
                table: "Empresas",
                column: "ResponsavelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_AspNetUsers_ResponsavelId",
                table: "Empresas");

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
                name: "CaptacaoContratos");

            migrationBuilder.DropTable(
                name: "CaptacaoSugestoesFornecedores");

            migrationBuilder.DropTable(
                name: "CaptacoesFornecedores");

            migrationBuilder.DropTable(
                name: "CategoriaContabilAtividades");

            migrationBuilder.DropTable(
                name: "Clausulas");

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
                name: "FaseTipoDetalhado");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "Propostas");

            migrationBuilder.DropTable(
                name: "Segmentos");

            migrationBuilder.DropTable(
                name: "SystemOptions");

            migrationBuilder.DropTable(
                name: "Temas");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "CategoriasContabeis");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "DemandaFormValues");

            migrationBuilder.DropTable(
                name: "FasesCadeiaProduto");

            migrationBuilder.DropTable(
                name: "Captacoes");

            migrationBuilder.DropTable(
                name: "Contratos");

            migrationBuilder.DropTable(
                name: "Demandas");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
