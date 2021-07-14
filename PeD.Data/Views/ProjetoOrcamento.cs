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
       IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.Nome, RECURSOHUMANO.NomeCompleto)       AS Recurso,
       IIF(A.Tipo = 'AlocacaoRm', CC.Nome, 'Recursos Humanos')                            AS CategoriaContabil,
       IIF(A.Tipo = 'AlocacaoRm', CC.Valor, 'RH')                                         AS CategoriaContabilCodigo,

       A.EmpresaFinanciadoraId                                                            as FinanciadorId,

       COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial)                           as Financiador,


       IIF(A.Tipo = 'AlocacaoRm', A.EmpresaRecebedoraId, RECURSOHUMANO.EmpresaId)         AS RecebedoraId,

       CASE
           WHEN A.Tipo = 'AlocacaoRm' THEN COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial)
           ELSE COALESCE(EmpresaRHRef.Nome, EmpresaRH.RazaoSocial)
           END                                                                            as Recebedor,

       IIF(A.Tipo = 'AlocacaoRm', A.Quantidade, ALOC_HORAS.Horas)                         AS Quantidade,
       IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.ValorUnitario, RECURSOHUMANO.ValorHora) AS Custo,
       IIF(A.Tipo = 'AlocacaoRm', CAST(A.Quantidade * RECURSOMATERIAL.ValorUnitario AS DECIMAL(18, 2)),
           CAST(ALOC_HORAS.Horas * RECURSOHUMANO.ValorHora as DECIMAL(18, 2)))            AS Total


FROM ProjetosRecursosAlocacoes A
         LEFT JOIN ProjetosAlocacaoRhHorasMeses ALOC_HORAS on A.Id = ALOC_HORAS.AlocacaoRhId
         LEFT JOIN ProjetoEtapas ETAPAS on A.EtapaId = ETAPAS.Id

    --Recursos
         LEFT JOIN ProjetoRecursosHumanos RECURSOHUMANO on A.RecursoHumanoId = RECURSOHUMANO.Id
         LEFT JOIN ProjetoRecursosMateriais RECURSOMATERIAL on A.RecursoMaterialId = RECURSOMATERIAL.Id
         LEFT JOIN CategoriasContabeis CC on RECURSOMATERIAL.CategoriaContabilId = CC.Id
    --Financiadores
         LEFT JOIN ProjetoEmpresas Financiadora on Financiadora.Id = A.EmpresaFinanciadoraId
         LEFT JOIN Empresas FinanciadoraRef on Financiadora.EmpresaRefId = FinanciadoraRef.Id
    -- RECEBEDORES
         LEFT JOIN ProjetoEmpresas Recebedora on A.EmpresaRecebedoraId = Recebedora.Id
         LEFT JOIN Empresas RecebedoraRef on Recebedora.EmpresaRefId = RecebedoraRef.Id

    -- RECEBEORES RH
         LEFT JOIN ProjetoEmpresas EmpresaRH on RECURSOHUMANO.EmpresaId = EmpresaRH.Id
         LEFT JOIN Empresas EmpresaRHRef on EmpresaRH.EmpresaRefId = EmpresaRHRef.Id
";
    }
}