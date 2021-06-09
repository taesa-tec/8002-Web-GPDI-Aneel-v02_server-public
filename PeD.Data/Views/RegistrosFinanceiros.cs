namespace PeD.Data.Views
{
    public class RegistrosFinanceiros : IView
    {
        public string CreateView => Up;

        public static string Up = @"
CREATE OR ALTER VIEW RegistrosFinanceirosView as
SELECT prf.Id,
       prf.Tipo,
       prf.ProjetoId,
       prf.NomeItem,
       prf.Beneficiado,
       prf.CnpjBeneficiado,
       prf.EquipaLaboratorioExistente,
       prf.EquipaLaboratorioNovo,
       prf.IsNacional,
       prf.MesReferencia,



       prf.RecursoHumanoId,
       prf.RecursoMaterialId,
       IIF(prf.Tipo = 'RegistroFinanceiroRm', PRM.Nome, PRH.NomeCompleto)                  AS Recurso,
       prf.Status,
       prf.TipoDocumento,
       prf.NumeroDocumento,
       prf.DataDocumento,
       prf.FinanciadoraId,
       prf.CoExecutorFinanciadorId,
       IIF(prf.FinanciadoraId IS NULL, CONCAT('c-', prf.CoExecutorFinanciadorId),CONCAT('e-', prf.FinanciadoraId)) as FinanciadorCode,


       IIF(prf.Tipo = 'RegistroFinanceiroRm', prf.RecebedoraId, PRH.EmpresaId)             AS RecebedoraId,

       IIF(prf.Tipo = 'RegistroFinanceiroRm', prf.CoExecutorRecebedorId, PRH.CoExecutorId) AS CoExecutorRecebedorId,

       CASE
           WHEN prf.FinanciadoraId IS NOT NULL THEN EF.Nome
           WHEN prf.CoExecutorFinanciadorId IS NOT NULL THEN PCEF.RazaoSocial
           END                                                                             as Financiador,

       CASE
           WHEN prf.Tipo = 'RegistroFinanceiroRm' THEN
               CASE
                   WHEN prf.RecebedoraId IS NOT NULL THEN ER.Nome
                   WHEN prf.CoExecutorRecebedorId IS NOT NULL THEN PCER.RazaoSocial
                   END
           ELSE
               CASE
                   WHEN PRH.EmpresaId IS NOT NULL THEN EH.Nome
                   WHEN PRH.CoExecutorId IS NOT NULL THEN PCEH.RazaoSocial
                   END
           END
                                                                                           as Recebedor,
       prf.CategoriaContabilId,
       IIF(prf.Tipo = 'RegistroFinanceiroRh', 'Recursos Humanos', CC.Nome)                 as CategoriaContabil,
       IIF(prf.Tipo = 'RegistroFinanceiroRh', 'RH', CC.Valor)                 as CategoriaContabilCodigo,
       COALESCE(prf.Valor, 0)                                                              as Valor,
       CASE
           WHEN prf.Tipo = 'RegistroFinanceiroRm' THEN COALESCE(prf.Valor * prf.Quantidade, 0)
           ELSE COALESCE(prf.Valor * prf.Horas, 0)
           END                                                                             AS Custo,
       IIF(prf.Tipo = 'RegistroFinanceiroRm', prf.Quantidade, prf.Horas)                   as QuantidadeHoras,

       prf.AtividadeRealizada,
       prf.EspecificaoTecnica,
       prf.FuncaoEtapa


FROM ProjetosRegistrosFinanceiros prf

         LEFT JOIN CategoriasContabeis CC on CC.Id = prf.CategoriaContabilId
         LEFT JOIN ProjetoRecursosHumanos PRH on prf.RecursoHumanoId = PRH.Id
         LEFT JOIN ProjetoRecursosMateriais PRM on prf.RecursoMaterialId = PRM.Id

         LEFT JOIN Empresas EF on EF.Id = prf.FinanciadoraId
         LEFT JOIN ProjetoCoExecutores PCEF on prf.CoExecutorFinanciadorId = PCEF.Id

         LEFT JOIN Empresas ER on ER.Id = prf.RecebedoraId
         LEFT JOIN ProjetoCoExecutores PCER on prf.CoExecutorRecebedorId = PCER.Id

         LEFT JOIN Empresas EH on EH.Id = PRH.EmpresaId
         LEFT JOIN ProjetoCoExecutores PCEH on PCEH.Id = PRH.CoExecutorId
";

        public static string Down = "";
    }
}