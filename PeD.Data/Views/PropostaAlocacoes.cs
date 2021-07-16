namespace PeD.Data.Views
{
    public class PropostaOrcamento : IView
    {
        public string CreateView => @"
CREATE OR ALTER VIEW PropostaAlocacoesView AS
SELECT 'RH'                                                                     as Categoria,
       Alocacao.PropostaId,
       Alocacao.EtapaId,
       Etapas.Ordem                                                             as EtapaOrdem,
       Alocacao.EmpresaFinanciadoraId,
       COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial)                 as EmpresaFinanciadora,
       Financiadora.Funcao                                                      AS EmpresaFinanciadoraFuncao,
       Alocacao.Justificativa,
       Alocacao.RecursoId,
       RecursoHumano.NomeCompleto                                               as Recurso,
       RecursoHumano.EmpresaId                                                  as EmpresaRecebedoraId,
       COALESCE(RecursoHumanoEmpresaRef.Nome, RecursoHumanoEmpresa.RazaoSocial) as EmpresaRecebedora,
       RecursoHumanoEmpresa.Funcao                                              as EmpresaRecebedoraFuncao,
       SUM(HorasMeses.Horas)                                                    as Quantidade,
       RecursoHumano.ValorHora                                                  as Valor,
       RecursoHumano.ValorHora * SUM(HorasMeses.Horas)                          as Custo
FROM PropostaRecursosHumanosAlocacao Alocacao
         INNER JOIN PropostaEtapas Etapas on Etapas.Id = Alocacao.EtapaId
         INNER JOIN PropostaRecursosHumanos RecursoHumano on RecursoHumano.Id = Alocacao.RecursoId
         INNER JOIN PropostaEmpresas Financiadora on Financiadora.Id = Alocacao.EmpresaFinanciadoraId
         INNER JOIN Empresas FinanciadoraRef on FinanciadoraRef.Id = Financiadora.EmpresaRefId
         INNER JOIN PropostaEmpresas RecursoHumanoEmpresa on RecursoHumano.EmpresaId = RecursoHumanoEmpresa.Id
         INNER JOIN Empresas RecursoHumanoEmpresaRef on RecursoHumanoEmpresaRef.Id = RecursoHumanoEmpresa.EmpresaRefId
         LEFT JOIN PropostasAlocacaoRhHorasMeses HorasMeses on Alocacao.Id = HorasMeses.AlocacaoRhId
GROUP BY Alocacao.Id, Alocacao.PropostaId, Alocacao.EtapaId, Alocacao.EmpresaFinanciadoraId,
         Alocacao.Justificativa, Alocacao.RecursoId, RecursoHumano.EmpresaId, ValorHora, Financiadora.RazaoSocial,
         FinanciadoraRef.Nome, RecursoHumanoEmpresaRef.Nome, RecursoHumanoEmpresa.RazaoSocial, Etapas.Ordem,
         RecursoHumano.NomeCompleto, RecursoHumanoEmpresa.Funcao, Financiadora.Funcao

UNION ALL

SELECT Categoria.Valor                                          as Categoria,
       Alocacao.PropostaId,
       Alocacao.EtapaId,
       Etapas.Ordem,
       Alocacao.EmpresaFinanciadoraId,
       COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial) as EmpresaFinanciadora,
       Financiadora.Funcao                                      as FinanciadoraFuncao,
       Alocacao.Justificativa,
       Alocacao.RecursoId,
       Recurso.Nome                                             as Recurso,
       Alocacao.EmpresaRecebedoraId,
       COALESCE(RecebedoraRef.Nome, Recebedora.RazaoSocial)     as EmpresaRecebedora,
       Recebedora.Funcao                                        as RecebedoraFuncao,
       Alocacao.Quantidade,
       Recurso.ValorUnitario                                    as Valor,
       Recurso.ValorUnitario * Alocacao.Quantidade
FROM PropostaRecursosMateriaisAlocacao Alocacao
         INNER JOIN PropostaRecursosMateriais Recurso on Alocacao.RecursoId = Recurso.Id
         INNER JOIN CategoriasContabeis Categoria on Categoria.Id = Recurso.CategoriaContabilId
         INNER JOIN PropostaEmpresas Financiadora on Financiadora.Id = Alocacao.EmpresaFinanciadoraId
         INNER JOIN Empresas FinanciadoraRef on FinanciadoraRef.Id = Financiadora.EmpresaRefId
         INNER JOIN PropostaEmpresas Recebedora on Recebedora.Id = Alocacao.EmpresaRecebedoraId
         INNER JOIN Empresas RecebedoraRef on RecebedoraRef.Id = Recebedora.EmpresaRefId
         INNER JOIN PropostaEtapas Etapas on Etapas.Id = Alocacao.EtapaId
";
    }
}