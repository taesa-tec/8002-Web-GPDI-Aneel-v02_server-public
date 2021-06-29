using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class ProjetosRelatorios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "PropriedadeIntelectualDepositante",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropriedadeId = table.Column<int>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: true),
                    CoExecutorId = table.Column<int>(nullable: true),
                    Porcentagem = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropriedadeIntelectualDepositante", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropriedadeIntelectualDepositante_ProjetoCoExecutores_CoExecutorId",
                        column: x => x.CoExecutorId,
                        principalTable: "ProjetoCoExecutores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropriedadeIntelectualDepositante_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropriedadeIntelectualDepositante_ProjetosRelatoriosPropriedadesIntelectuais_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalTable: "ProjetosRelatoriosPropriedadesIntelectuais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_PropriedadeIntelectualDepositante_CoExecutorId",
                table: "PropriedadeIntelectualDepositante",
                column: "CoExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_PropriedadeIntelectualDepositante_EmpresaId",
                table: "PropriedadeIntelectualDepositante",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_PropriedadeIntelectualDepositante_PropriedadeId",
                table: "PropriedadeIntelectualDepositante",
                column: "PropriedadeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "PropriedadeIntelectualDepositante");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuais");
        }
    }
}
