using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class ProjetosRelatorios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Segmentos");

            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "Segmentos",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Compartilhamento",
                table: "Projetos",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegmentoId",
                table: "Projetos",
                nullable: true,
                defaultValue: "G");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos",
                column: "Valor");

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

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Açores" },
                    { 165, "Mayotte" },
                    { 166, "México" },
                    { 167, "Micronésia" },
                    { 168, "Moçambique" },
                    { 169, "Moldávia" },
                    { 170, "Mônaco" },
                    { 171, "Mongólia" },
                    { 172, "Monserrate" },
                    { 173, "Montenegro" },
                    { 174, "Myanmar" },
                    { 175, "Namíbia" },
                    { 176, "Nauru" },
                    { 177, "Nepal" },
                    { 164, "Mauritânia" },
                    { 178, "Nicarágua" },
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
                    { 190, "Papua-Nova Guiné" },
                    { 191, "Paquistão" },
                    { 192, "Paraguai" },
                    { 179, "Níger" },
                    { 163, "Maurício" },
                    { 162, "Martinica" },
                    { 161, "Marrocos" },
                    { 132, "Irlanda" },
                    { 133, "Irlanda do norte" },
                    { 134, "Islândia" },
                    { 135, "Israel" },
                    { 136, "Itália" },
                    { 137, "Jamaica" },
                    { 138, "Japão" },
                    { 139, "Jersey" },
                    { 140, "Jordânia" },
                    { 141, "Kosovo" },
                    { 142, "Kuwait" },
                    { 143, "Laos" },
                    { 144, "Lesoto" },
                    { 145, "Letônia" },
                    { 146, "Líbano" },
                    { 147, "Libéria" },
                    { 148, "Líbia" },
                    { 149, "Liechtenstein" },
                    { 150, "Lituânia" },
                    { 151, "Luxemburgo" },
                    { 152, "Macau" },
                    { 153, "Macedônia" },
                    { 154, "Madagascar" },
                    { 155, "Malásia" },
                    { 156, "Malawi" },
                    { 157, "Maldivas" },
                    { 158, "Mali" },
                    { 159, "Malta" },
                    { 160, "Marianas do Norte" },
                    { 193, "Peru" },
                    { 131, "Iraque" },
                    { 194, "Polinésia Francesa" },
                    { 196, "Porto Rico" },
                    { 230, "Sudão do Sul" },
                    { 231, "Suécia" },
                    { 232, "Suíça" },
                    { 233, "Suriname" },
                    { 234, "Tailândia" },
                    { 235, "Taiwan" },
                    { 236, "Tajiquistão" },
                    { 237, "Tanzânia" },
                    { 238, "Território Britânico do Oceano Índico" },
                    { 239, "Territórios Austrais Franceses" },
                    { 240, "Timor Leste" },
                    { 241, "Togo" },
                    { 242, "Tokelau" },
                    { 229, "Sudão" },
                    { 243, "Tonga" },
                    { 245, "Tunísia" },
                    { 246, "Turquemenistão" },
                    { 247, "Turquia" },
                    { 248, "Tuvalu" },
                    { 249, "Ucrânia" },
                    { 250, "Uganda" },
                    { 251, "Uruguai" },
                    { 252, "Uzbequistão" },
                    { 253, "Vanuatu" },
                    { 254, "Vaticano" },
                    { 255, "Venezuela" },
                    { 256, "Vietnã" },
                    { 257, "Wallis e Futuna" },
                    { 244, "Trindade e Tobago" },
                    { 228, "Suazilândia" },
                    { 227, "Sri Lanka" },
                    { 226, "Somália" },
                    { 197, "Portugal" },
                    { 198, "Quênia" },
                    { 199, "Quirguizistão" },
                    { 200, "Quiribati" },
                    { 201, "Reino Unido" },
                    { 202, "República Centro-Africana" },
                    { 203, "República Checa" },
                    { 204, "República Democrática do Congo" },
                    { 205, "República do Congo" },
                    { 206, "República Dominicana" },
                    { 207, "Romênia" },
                    { 208, "Ruanda" },
                    { 209, "Rússia" },
                    { 210, "Saara Ocidental" },
                    { 211, "Samoa" },
                    { 212, "Samoa Americana" },
                    { 213, "Santa Helena" },
                    { 214, "Santa Lúcia" },
                    { 215, "São Cristóvão e Neves" },
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
                    { 195, "Polônia" },
                    { 258, "Zâmbia" },
                    { 130, "Irã" },
                    { 128, "Indonésia" },
                    { 35, "Bulgária" },
                    { 36, "Burquina Faso" },
                    { 37, "Burundi" },
                    { 38, "Butão" },
                    { 39, "Cabo Verde" },
                    { 40, "Camarões" },
                    { 41, "Camboja" },
                    { 42, "Canadá" },
                    { 43, "Canárias" },
                    { 44, "Catar" },
                    { 45, "Cazaquistão" },
                    { 46, "Chade" },
                    { 47, "Chile" },
                    { 34, "Brunei" },
                    { 48, "China" },
                    { 50, "Colômbia" },
                    { 51, "Comores" },
                    { 52, "Coreia do Norte" },
                    { 53, "Coreia do Sul" },
                    { 54, "Costa do Marfim" },
                    { 55, "Costa Rica" },
                    { 56, "Croácia" },
                    { 57, "Cuba" },
                    { 58, "Curaçao" },
                    { 59, "Dinamarca" },
                    { 60, "Djibuti" },
                    { 61, "Domínica" },
                    { 62, "Egito" },
                    { 49, "Chipre" },
                    { 33, "Brasil" },
                    { 32, "Botsuana" },
                    { 31, "Bósnia e Herzegovina" },
                    { 2, "Acrotiri e Deceleia" },
                    { 3, "Afeganistão" },
                    { 4, "África do Sul" },
                    { 5, "Albânia" },
                    { 6, "Alemanha" },
                    { 7, "Andorra" },
                    { 8, "Angola" },
                    { 9, "Anguila" },
                    { 10, "Antártida" },
                    { 11, "Antígua e Barbuda" },
                    { 12, "Antilhas Holandesas" },
                    { 13, "Arábia Saudita" },
                    { 14, "Argélia" },
                    { 15, "Argentina" },
                    { 16, "Armênia" },
                    { 17, "Aruba" },
                    { 18, "Austrália" },
                    { 19, "Áustria" },
                    { 20, "Azerbaijão" },
                    { 21, "Bahamas" },
                    { 22, "Bangladesh" },
                    { 23, "Barbados" },
                    { 24, "Barém" },
                    { 25, "Bélgica" },
                    { 26, "Belize" },
                    { 27, "Benim" },
                    { 28, "Bermudas" },
                    { 29, "Bielorrússia" },
                    { 30, "Bolívia" },
                    { 63, "El Salvador" },
                    { 129, "Inglaterra" },
                    { 64, "Emirados Árabes Unidos" },
                    { 66, "Eritreia" },
                    { 100, "Hong Kong" },
                    { 101, "Hungria" },
                    { 102, "Iêmen" },
                    { 103, "Ilha Bouvet" },
                    { 104, "Ilha da Madeira" },
                    { 105, "Ilha de Clipperton" },
                    { 106, "Ilha de Man" },
                    { 107, "Ilha de Navassa" },
                    { 108, "Ilha do Natal" },
                    { 109, "Ilha Jan Mayen" },
                    { 110, "Ilha Norfolk" },
                    { 111, "Ilha Wake" },
                    { 112, "Ilhas Ashmore e Cartier" },
                    { 99, "Honduras" },
                    { 113, "Ilhas Caimão" },
                    { 115, "Ilhas Cook" },
                    { 116, "Ilhas do mar de coral" },
                    { 117, "Ilhas Falkland" },
                    { 118, "Ilhas Heard e McDonald" },
                    { 119, "Ilhas Marshall" },
                    { 120, "Ilhas Paracel" },
                    { 121, "Ilhas Pitcairn" },
                    { 122, "Ilhas Salomão" },
                    { 123, "Ilhas Spratly" },
                    { 124, "Ilhas Turcas e Caicos" },
                    { 125, "Ilhas Virgens Americanas" },
                    { 126, "Ilhas Virgens Britânicas" },
                    { 127, "Índia" },
                    { 114, "Ilhas Cocos" },
                    { 98, "Holanda (Países baixos)" },
                    { 97, "Haiti" },
                    { 96, "Guiné Equatorial" },
                    { 67, "Escócia" },
                    { 68, "Eslováquia" },
                    { 69, "Eslovênia" },
                    { 70, "Espanha" },
                    { 71, "Estados Unidos" },
                    { 72, "Estônia" },
                    { 73, "Etiópia" },
                    { 74, "Faroé" },
                    { 75, "Fiji" },
                    { 76, "Filipinas" },
                    { 77, "Finlândia" },
                    { 78, "França" },
                    { 79, "Gabão" },
                    { 80, "Gâmbia" },
                    { 81, "Gana" },
                    { 82, "Geórgia" },
                    { 83, "Geórgia do Sul e Sandwich do Sul" },
                    { 84, "Gibraltar" },
                    { 85, "Granada" },
                    { 86, "Grécia" },
                    { 87, "Groenlândia" },
                    { 88, "Guadalupe" },
                    { 89, "Guam" },
                    { 90, "Guatemala" },
                    { 91, "Guernsey" },
                    { 92, "Guiana" },
                    { 93, "Guiana Francesa" },
                    { 94, "Guiné" },
                    { 95, "Guiné Bissau" },
                    { 65, "Equador" },
                    { 259, "Zimbabué" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_SegmentoId",
                table: "Projetos",
                column: "SegmentoId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Segmentos_SegmentoId",
                table: "Projetos",
                column: "SegmentoId",
                principalTable: "Segmentos",
                principalColumn: "Valor",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Segmentos_SegmentoId",
                table: "Projetos");

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
                name: "PropriedadeIntelectualDepositante");

            migrationBuilder.DropTable(
                name: "ProjetosRelatoriosPropriedadesIntelectuais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos");

            migrationBuilder.DropIndex(
                name: "IX_Projetos_SegmentoId",
                table: "Projetos");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "C");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "D");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "G");

            migrationBuilder.DeleteData(
                table: "Segmentos",
                keyColumn: "Valor",
                keyValue: "T");

            migrationBuilder.DropColumn(
                name: "Compartilhamento",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "SegmentoId",
                table: "Projetos");

            migrationBuilder.AlterColumn<string>(
                name: "Valor",
                table: "Segmentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Segmentos",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Segmentos",
                table: "Segmentos",
                column: "Id");

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
        }
    }
}
