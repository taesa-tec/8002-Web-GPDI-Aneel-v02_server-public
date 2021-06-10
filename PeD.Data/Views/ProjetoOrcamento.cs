namespace PeD.Data.Views
{
    public class ProjetoOrcamento : IView
    {
        public string CreateView => @"
CREATE OR ALTER VIEW ProjetoOrcamentoView as
SELECT A.Id,
       A.ProjetoId,
       A.Tipo,
       A.EtapaId,
       ETAPAS.Ordem,
       ALOC_HORAS.Mes,
       A.RecursoHumanoId,
       A.RecursoMaterialId,
       IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.Nome, RECURSOHUMANO.NomeCompleto)    AS Recurso,
       IIF(A.Tipo = 'AlocacaoRm', CC.Nome, 'Recursos Humanos')                       AS CategoriaContabil,
       IIF(A.Tipo = 'AlocacaoRm', CC.Valor, 'RH')                       AS CategoriaContabilCodigo,

       A.EmpresaFinanciadoraId,
       A.CoExecutorFinanciadorId,
       IIF(A.EmpresaFinanciadoraId IS NULL, CONCAT('c-', A.CoExecutorFinanciadorId),CONCAT('e-', A.EmpresaFinanciadoraId)) as FinanciadorCode,
       CASE
           WHEN A.EmpresaFinanciadoraId IS NOT NULL THEN E_FIN.Nome
           WHEN A.CoExecutorFinanciadorId IS NOT NULL THEN PCE_FIN.RazaoSocial
           END                                                                         as Financiador,


       IIF(A.Tipo = 'AlocacaoRm', A.EmpresaRecebedoraId, RECURSOHUMANO.EmpresaId)      AS RecebedoraId,
       IIF(A.Tipo = 'AlocacaoRm', A.CoExecutorRecebedorId, RECURSOHUMANO.CoExecutorId) AS CoExecutorRecebedorId,
       CASE
           WHEN A.Tipo = 'AlocacaoRm' THEN
               CASE
                   WHEN A.EmpresaRecebedoraId IS NOT NULL THEN E_FIN.Nome
                   WHEN A.CoExecutorRecebedorId IS NOT NULL THEN PCE_REC.RazaoSocial
                   END
           ELSE
               CASE
                   WHEN RECURSOHUMANO.EmpresaId IS NOT NULL THEN E_RH.Nome
                   WHEN RECURSOHUMANO.CoExecutorId IS NOT NULL THEN PCE_RH.RazaoSocial
                   END
           END                                                                         as Recebedor,

       IIF(A.Tipo = 'AlocacaoRm', A.Quantidade, ALOC_HORAS.Horas)                      AS Quantidade,
       IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.ValorUnitario, RECURSOHUMANO.ValorHora) AS Custo,
       IIF(A.Tipo = 'AlocacaoRm', CAST(A.Quantidade * RECURSOMATERIAL.ValorUnitario AS DECIMAL(18,2)),
           CAST(ALOC_HORAS.Horas * RECURSOHUMANO.ValorHora as DECIMAL(18,2)))                                 AS Total


FROM ProjetosRecursosAlocacoes A
         LEFT JOIN ProjetosAlocacaoRhHorasMeses ALOC_HORAS on A.Id = ALOC_HORAS.AlocacaoRhId
         LEFT JOIN ProjetoEtapas ETAPAS on A.EtapaId = ETAPAS.Id

    --Recursos
         LEFT JOIN ProjetoRecursosHumanos RECURSOHUMANO on A.RecursoHumanoId = RECURSOHUMANO.Id
         LEFT JOIN ProjetoRecursosMateriais RECURSOMATERIAL on A.RecursoMaterialId = RECURSOMATERIAL.Id
         LEFT JOIN CategoriasContabeis CC on RECURSOMATERIAL.CategoriaContabilId = CC.Id
    --Financiadores
         LEFT JOIN ProjetoCoExecutores PCE_FIN on PCE_FIN.Id = A.CoExecutorFinanciadorId
         LEFT JOIN Empresas E_FIN on E_FIN.Id = A.EmpresaFinanciadoraId
    -- RECEBEDORES
         LEFT JOIN ProjetoCoExecutores PCE_REC on PCE_REC.Id = A.CoExecutorRecebedorId
         LEFT JOIN Empresas E_REC on A.EmpresaRecebedoraId = E_REC.Id
    -- RECEBEORES RH
         LEFT JOIN ProjetoCoExecutores PCE_RH on PCE_RH.Id = RECURSOHUMANO.CoExecutorId
         LEFT JOIN Empresas E_RH on RECURSOHUMANO.EmpresaId = E_RH.Id


";
    }
}