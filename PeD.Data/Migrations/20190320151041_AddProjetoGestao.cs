using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class AddProjetoGestao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Projetos_ProjetoId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_EtapaProdutos_Etapas_EtapaId",
                table: "EtapaProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Projetos_ProjetoId",
                table: "Etapas");

            migrationBuilder.DropForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Projetos_ProjetoId",
                table: "Produtos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoHumanos_Empresas_EmpresaId",
                table: "RecursoHumanos");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_Empresas_EmpresaId",
                table: "ResultadoIntelectualDepositantes");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualDepositantes");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualInventores_RecursoHumanos_RecursoHumanoId",
                table: "ResultadoIntelectualInventores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualInventores_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualInventores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCapacitacao_RecursoHumanos_RecursoHumanoId",
                table: "ResultadosCapacitacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosProducao_CatalogPaises_CatalogPaisId",
                table: "ResultadosProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_CatalogTema_CatalogTemaId",
                table: "Temas");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas");

            migrationBuilder.DropForeignKey(
                name: "FK_TemaSubTemas_CatalogSubTemas_CatalogSubTemaId",
                table: "TemaSubTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjetos_CatalogUserPermissoes_CatalogUserPermissaoId",
                table: "UserProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos");

            migrationBuilder.AddColumn<int>(
                name: "CatalogAtividadeId",
                table: "RecursoMateriais",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GerenteProjeto",
                table: "RecursoHumanos",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes10",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes11",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes12",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes13",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes14",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes15",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes16",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes17",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes18",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes19",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes20",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes21",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes22",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes23",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes24",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes7",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes8",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HrsMes9",
                table: "AlocacoesRh",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AtividadesGestao",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjetoId = table.Column<int>(nullable: false),
                    DedicacaoHorario = table.Column<string>(nullable: true),
                    ParticipacaoMembros = table.Column<string>(nullable: true),
                    DesenvFerramenta = table.Column<string>(nullable: true),
                    ProspTecnologica = table.Column<string>(nullable: true),
                    DivulgacaoResultados = table.Column<string>(nullable: true),
                    ParticipacaoTecnicos = table.Column<string>(nullable: true),
                    BuscaAnterioridade = table.Column<string>(nullable: true),
                    ContratacaoAuditoria = table.Column<string>(nullable: true),
                    ApoioCitenel = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadesGestao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaContabil",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCategoriaContabilGestao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EtapaMeses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EtapaId = table.Column<int>(nullable: false),
                    Mes = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtapaMeses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtapaMeses_Etapas_EtapaId",
                        column: x => x.EtapaId,
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atividade",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: true),
                    Valor = table.Column<string>(nullable: true),
                    CatalogCategoriaContabilGestaoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogAtividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogAtividade_CatalogCategoriaContabilGestao_CatalogCategoriaContabilGestaoId",
                        column: x => x.CatalogCategoriaContabilGestaoId,
                        principalTable: "CategoriaContabil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosSocioAmbiental_ProjetoId",
                table: "ResultadosSocioAmbiental",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosProducao_ProjetoId",
                table: "ResultadosProducao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosIntelectual_ProjetoId",
                table: "ResultadosIntelectual",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosInfra_ProjetoId",
                table: "ResultadosInfra",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosEconomico_ProjetoId",
                table: "ResultadosEconomico",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosCapacitacao_ProjetoId",
                table: "ResultadosCapacitacao",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosFinanceiros_ProjetoId",
                table: "RegistrosFinanceiros",
                column: "ProjetoId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursoMateriais_CatalogAtividadeId",
                table: "RecursoMateriais",
                column: "CatalogAtividadeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecursoMateriais_CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais",
                column: "CatalogCategoriaContabilGestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogAtividade_CatalogCategoriaContabilGestaoId",
                table: "Atividade",
                column: "CatalogCategoriaContabilGestaoId");

            migrationBuilder.CreateIndex(
                name: "IX_EtapaMeses_EtapaId",
                table: "EtapaMeses",
                column: "EtapaId");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas",
                column: "CatalogTemaId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Projetos_ProjetoId",
                table: "Empresas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EtapaProdutos_Etapas_EtapaId",
                table: "EtapaProdutos",
                column: "EtapaId",
                principalTable: "Etapas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Projetos_ProjetoId",
                table: "Etapas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Projetos_ProjetoId",
                table: "Produtos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoHumanos_Empresas_EmpresaId",
                table: "RecursoHumanos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoMateriais_CatalogAtividade_CatalogAtividadeId",
                table: "RecursoMateriais",
                column: "CatalogAtividadeId",
                principalTable: "Atividade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoMateriais_CatalogCategoriaContabilGestao_CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais",
                column: "CatalogCategoriaContabilGestaoId",
                principalTable: "CategoriaContabil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosFinanceiros_Projetos_ProjetoId",
                table: "RegistrosFinanceiros",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_Empresas_EmpresaId",
                table: "ResultadoIntelectualDepositantes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualDepositantes",
                column: "ResultadoIntelectualId",
                principalTable: "ResultadosIntelectual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualInventores_RecursoHumanos_RecursoHumanoId",
                table: "ResultadoIntelectualInventores",
                column: "RecursoHumanoId",
                principalTable: "RecursoHumanos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualInventores_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualInventores",
                column: "ResultadoIntelectualId",
                principalTable: "ResultadosIntelectual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCapacitacao_Projetos_ProjetoId",
                table: "ResultadosCapacitacao",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCapacitacao_RecursoHumanos_RecursoHumanoId",
                table: "ResultadosCapacitacao",
                column: "RecursoHumanoId",
                principalTable: "RecursoHumanos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosEconomico_Projetos_ProjetoId",
                table: "ResultadosEconomico",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosInfra_Projetos_ProjetoId",
                table: "ResultadosInfra",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosIntelectual_Projetos_ProjetoId",
                table: "ResultadosIntelectual",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosProducao_CatalogPaises_CatalogPaisId",
                table: "ResultadosProducao",
                column: "CatalogPaisId",
                principalTable: "CatalogPaises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosProducao_Projetos_ProjetoId",
                table: "ResultadosProducao",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosSocioAmbiental_Projetos_ProjetoId",
                table: "ResultadosSocioAmbiental",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_CatalogTema_CatalogTemaId",
                table: "Temas",
                column: "CatalogTemaId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TemaSubTemas_CatalogSubTemas_CatalogSubTemaId",
                table: "TemaSubTemas",
                column: "CatalogSubTemaId",
                principalTable: "CatalogSubTemas",
                principalColumn: "SubTemaId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjetos_CatalogUserPermissoes_CatalogUserPermissaoId",
                table: "UserProjetos",
                column: "CatalogUserPermissaoId",
                principalTable: "CatalogUserPermissoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Projetos_ProjetoId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_EtapaProdutos_Etapas_EtapaId",
                table: "EtapaProdutos");

            migrationBuilder.DropForeignKey(
                name: "FK_Etapas_Projetos_ProjetoId",
                table: "Etapas");

            migrationBuilder.DropForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Projetos_ProjetoId",
                table: "Produtos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoHumanos_Empresas_EmpresaId",
                table: "RecursoHumanos");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoMateriais_CatalogAtividade_CatalogAtividadeId",
                table: "RecursoMateriais");

            migrationBuilder.DropForeignKey(
                name: "FK_RecursoMateriais_CatalogCategoriaContabilGestao_CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosFinanceiros_Projetos_ProjetoId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_Empresas_EmpresaId",
                table: "ResultadoIntelectualDepositantes");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualDepositantes");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualInventores_RecursoHumanos_RecursoHumanoId",
                table: "ResultadoIntelectualInventores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoIntelectualInventores_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualInventores");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCapacitacao_Projetos_ProjetoId",
                table: "ResultadosCapacitacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosCapacitacao_RecursoHumanos_RecursoHumanoId",
                table: "ResultadosCapacitacao");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosEconomico_Projetos_ProjetoId",
                table: "ResultadosEconomico");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosInfra_Projetos_ProjetoId",
                table: "ResultadosInfra");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosIntelectual_Projetos_ProjetoId",
                table: "ResultadosIntelectual");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosProducao_CatalogPaises_CatalogPaisId",
                table: "ResultadosProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosProducao_Projetos_ProjetoId",
                table: "ResultadosProducao");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadosSocioAmbiental_Projetos_ProjetoId",
                table: "ResultadosSocioAmbiental");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_CatalogTema_CatalogTemaId",
                table: "Temas");

            migrationBuilder.DropForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas");

            migrationBuilder.DropForeignKey(
                name: "FK_TemaSubTemas_CatalogSubTemas_CatalogSubTemaId",
                table: "TemaSubTemas");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjetos_CatalogUserPermissoes_CatalogUserPermissaoId",
                table: "UserProjetos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos");

            migrationBuilder.DropTable(
                name: "AtividadesGestao");

            migrationBuilder.DropTable(
                name: "Atividade");

            migrationBuilder.DropTable(
                name: "EtapaMeses");

            migrationBuilder.DropTable(
                name: "CategoriaContabil");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosSocioAmbiental_ProjetoId",
                table: "ResultadosSocioAmbiental");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosProducao_ProjetoId",
                table: "ResultadosProducao");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosIntelectual_ProjetoId",
                table: "ResultadosIntelectual");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosInfra_ProjetoId",
                table: "ResultadosInfra");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosEconomico_ProjetoId",
                table: "ResultadosEconomico");

            migrationBuilder.DropIndex(
                name: "IX_ResultadosCapacitacao_ProjetoId",
                table: "ResultadosCapacitacao");

            migrationBuilder.DropIndex(
                name: "IX_RegistrosFinanceiros_ProjetoId",
                table: "RegistrosFinanceiros");

            migrationBuilder.DropIndex(
                name: "IX_RecursoMateriais_CatalogAtividadeId",
                table: "RecursoMateriais");

            migrationBuilder.DropIndex(
                name: "IX_RecursoMateriais_CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais");

            migrationBuilder.DropColumn(
                name: "CatalogAtividadeId",
                table: "RecursoMateriais");

            migrationBuilder.DropColumn(
                name: "CatalogCategoriaContabilGestaoId",
                table: "RecursoMateriais");

            migrationBuilder.DropColumn(
                name: "GerenteProjeto",
                table: "RecursoHumanos");

            migrationBuilder.DropColumn(
                name: "HrsMes10",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes11",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes12",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes13",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes14",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes15",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes16",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes17",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes18",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes19",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes20",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes21",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes22",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes23",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes24",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes7",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes8",
                table: "AlocacoesRh");

            migrationBuilder.DropColumn(
                name: "HrsMes9",
                table: "AlocacoesRh");

            migrationBuilder.AddForeignKey(
                name: "FK_CatalogSubTemas_CatalogTema_CatalogTemaId",
                table: "CatalogSubTemas",
                column: "CatalogTemaId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Projetos_ProjetoId",
                table: "Empresas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EtapaProdutos_Etapas_EtapaId",
                table: "EtapaProdutos",
                column: "EtapaId",
                principalTable: "Etapas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Etapas_Projetos_ProjetoId",
                table: "Etapas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LogProjetos_Projetos_ProjetoId",
                table: "LogProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Projetos_ProjetoId",
                table: "Produtos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecursoHumanos_Empresas_EmpresaId",
                table: "RecursoHumanos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroObs_RegistrosFinanceiros_RegistroFinanceiroId",
                table: "RegistroObs",
                column: "RegistroFinanceiroId",
                principalTable: "RegistrosFinanceiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RelatorioFinal_Projetos_ProjetoId",
                table: "RelatorioFinal",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_Empresas_EmpresaId",
                table: "ResultadoIntelectualDepositantes",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualDepositantes_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualDepositantes",
                column: "ResultadoIntelectualId",
                principalTable: "ResultadosIntelectual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualInventores_RecursoHumanos_RecursoHumanoId",
                table: "ResultadoIntelectualInventores",
                column: "RecursoHumanoId",
                principalTable: "RecursoHumanos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoIntelectualInventores_ResultadosIntelectual_ResultadoIntelectualId",
                table: "ResultadoIntelectualInventores",
                column: "ResultadoIntelectualId",
                principalTable: "ResultadosIntelectual",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosCapacitacao_RecursoHumanos_RecursoHumanoId",
                table: "ResultadosCapacitacao",
                column: "RecursoHumanoId",
                principalTable: "RecursoHumanos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadosProducao_CatalogPaises_CatalogPaisId",
                table: "ResultadosProducao",
                column: "CatalogPaisId",
                principalTable: "CatalogPaises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_CatalogTema_CatalogTemaId",
                table: "Temas",
                column: "CatalogTemaId",
                principalTable: "Tema",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Temas_Projetos_ProjetoId",
                table: "Temas",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemaSubTemas_CatalogSubTemas_CatalogSubTemaId",
                table: "TemaSubTemas",
                column: "CatalogSubTemaId",
                principalTable: "CatalogSubTemas",
                principalColumn: "SubTemaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjetos_CatalogUserPermissoes_CatalogUserPermissaoId",
                table: "UserProjetos",
                column: "CatalogUserPermissaoId",
                principalTable: "CatalogUserPermissoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProjetos_Projetos_ProjetoId",
                table: "UserProjetos",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
