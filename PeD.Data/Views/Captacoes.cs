namespace PeD.Data.Views
{
    public class Captacoes : IView
    {
        public string CreateView => Up;

        public static string Up = @"CREATE OR ALTER VIEW CaptacoesView as
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

        public static string Down = "";
    }
}