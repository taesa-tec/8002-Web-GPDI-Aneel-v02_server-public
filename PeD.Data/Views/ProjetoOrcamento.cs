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
       A.RecursoHumanoId,
       A.RecursoMaterialId,
       IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.Nome, RECURSOHUMANO.NomeCompleto)                    AS Recurso,
       IIF(A.Tipo = 'AlocacaoRm', CC.Nome, 'Recursos Humanos')                                         AS CategoriaContabil,
       IIF(A.Tipo = 'AlocacaoRm', CC.Valor, 'RH')                                                      AS CategoriaContabilCodigo,

       A.EmpresaFinanciadoraId                                                                         as FinanciadoraId,
       COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial)                                        as Financiadora,


       IIF(A.Tipo = 'AlocacaoRm', A.EmpresaRecebedoraId, RECURSOHUMANO.EmpresaId)                      AS RecebedoraId,

       IIF(A.Tipo = 'AlocacaoRm', COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial),
           COALESCE(EmpresaRHRef.Nome, EmpresaRH.RazaoSocial))                                         as Recebedora,

       COALESCE(IIF(A.Tipo = 'AlocacaoRm', A.Quantidade, SUM(ALOC_HORAS.Horas)), 0)                    AS Quantidade,
       COALESCE(IIF(A.Tipo = 'AlocacaoRm', RECURSOMATERIAL.ValorUnitario, RECURSOHUMANO.ValorHora), 0) AS Custo,
       COALESCE(IIF(A.Tipo = 'AlocacaoRm', CAST(A.Quantidade * RECURSOMATERIAL.ValorUnitario AS DECIMAL(18, 2)),
                    CAST(Sum(ALOC_HORAS.Horas) * RECURSOHUMANO.ValorHora as DECIMAL(18, 2))), 0)       AS Total


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
GROUP BY A.Id, A.ProjetoId, A.Tipo, A.EtapaId, ETAPAS.Ordem, A.RecursoHumanoId, A.RecursoMaterialId,
         A.EmpresaFinanciadoraId,
         A.Quantidade, RECURSOMATERIAL.ValorUnitario,
         RECURSOHUMANO.ValorHora, RECURSOMATERIAL.Nome, RECURSOHUMANO.NomeCompleto, CC.Nome, CC.Valor,
         FinanciadoraRef.Nome, Financiadora.RazaoSocial, A.EmpresaRecebedoraId, RECURSOHUMANO.EmpresaId,
         EmpresaRHRef.Nome, EmpresaRH.RazaoSocial
";
    }
}