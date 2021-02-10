using Microsoft.EntityFrameworkCore.Migrations;

namespace PeD.Data.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR ALTER VIEW CaptacoesView as
SELECT
       C.Id,
       C.Titulo,
       C.CriadorId,
       ANU.NomeCompleto AS Criador,
       C.UsuarioSuprimentoId,
       A.NomeCompleto as UsuarioSuprimento,
       C.Status,
       COUNT(DISTINCT CF.FornecedorId) as TotalConvidados,
       COUNT(DISTINCT P.Id) as TotalPropostas,
       C.CreatedAt,
       C.EnvioCaptacao,
       C.Termino,
       C.Cancelamento
FROM Captacoes C
LEFT JOIN CaptacoesFornecedores CF on C.Id = CF.CaptacaoId
LEFT JOIN AspNetUsers ANU on ANU.Id = C.CriadorId
LEFT JOIN AspNetUsers A on A.Id = C.UsuarioSuprimentoId
LEFT JOIN Propostas P on C.Id = P.CaptacaoId  AND P.Finalizado = 1 AND P.Participacao = 1
GROUP BY C.Id, C.Titulo, C.CriadorId, ANU.NomeCompleto, C.CreatedAt, C.EnvioCaptacao, C.Termino, C.Cancelamento, C.Status, C.UsuarioSuprimentoId, A.NomeCompleto;";


            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}