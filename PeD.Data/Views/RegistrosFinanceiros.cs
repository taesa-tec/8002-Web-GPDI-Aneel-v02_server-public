namespace PeD.Data.Views
{
    public class RegistrosFinanceiros : IView
    {
        public string CreateView => Up;

        public static string Up = @"
CREATE OR ALTER VIEW RegistrosFinanceirosView as
SELECT Registro.Id,
       Registro.ProjetoId,
       Registro.AuthorId,
       Registro.Tipo,
       Registro.Status,
       Registro.Beneficiado,
       Registro.CnpjBeneficiado,
       Registro.EquipaLaboratorioExistente,
       Registro.EquipaLaboratorioNovo,
       Registro.IsNacional,
       Registro.MesReferencia,
       Registro.RecursoHumanoId,
       Registro.RecursoMaterialId,
       Registro.TipoDocumento,
       Registro.NumeroDocumento,
       Registro.DataDocumento,
       Registro.CategoriaContabilId,
       Registro.Valor                                                                         as Valor,
       Registro.AtividadeRealizada,
       Registro.EspecificaoTecnica,
       Etapa.Ordem                                                                            as Etapa,
       Registro.FuncaoEtapa,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm', RecursoM.Nome, RecursoH.NomeCompleto)      AS Recurso,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Registro.NomeItem, RecursoH.NomeCompleto)  as NomeItem,


       Registro.FinanciadoraId,
       Financiadora.Codigo                                                                    as FinanciadoraCodigo,
       Financiadora.Funcao                                                                    as FinanciadoraFuncao,
       Financiadora.CNPJ                                                                      as CNPJParc,
       COALESCE(FinanciadoraRef.Nome, Financiadora.RazaoSocial)                               as Financiadora,

       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Registro.RecebedoraId, RecursoH.EmpresaId) AS RecebedoraId,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Recebedora.Codigo, EmpresaRH.Codigo)       AS RecebedoraCodigo,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Recebedora.Funcao, EmpresaRH.Funcao)       AS RecebedoraFuncao,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm',
           COALESCE(RecebedoraRef.Nome, Recebedora.RazaoSocial),
           COALESCE(EmpresaRHRef.Nome, EmpresaRH.RazaoSocial))                              AS Recebedora,

       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Recebedora.Cnpj, EmpresaRH.Cnpj)           as CNPJExec,


       IIF(Registro.Tipo = 'RegistroFinanceiroRh', 'Recursos Humanos', Cat.Nome)              as CategoriaContabil,
       IIF(Registro.Tipo = 'RegistroFinanceiroRh', 'RH', Cat.Valor)                           as CategoriaContabilCodigo,


       IIF(Registro.Tipo = 'RegistroFinanceiroRm', COALESCE(Registro.Valor * Registro.Quantidade, 0),
           COALESCE(Registro.Valor * Registro.Horas, 0))                                      AS Custo,
       IIF(Registro.Tipo = 'RegistroFinanceiroRm', Registro.Quantidade, Registro.Horas)       as QuantidadeHoras


FROM ProjetosRegistrosFinanceiros Registro

         LEFT JOIN CategoriasContabeis Cat on Cat.Id = Registro.CategoriaContabilId
         LEFT JOIN ProjetoRecursosHumanos RecursoH on Registro.RecursoHumanoId = RecursoH.Id
         LEFT JOIN ProjetoRecursosMateriais RecursoM on Registro.RecursoMaterialId = RecursoM.Id
         LEFT JOIN ProjetoEmpresas Financiadora on Financiadora.Id = Registro.FinanciadoraId
         LEFT JOIN Empresas FinanciadoraRef on Financiadora.EmpresaRefId = FinanciadoraRef.Id
         LEFT JOIN ProjetoEmpresas Recebedora on Recebedora.Id = Registro.RecebedoraId
         LEFT JOIN Empresas RecebedoraRef on Recebedora.EmpresaRefId = RecebedoraRef.Id
         LEFT JOIN ProjetoEmpresas EmpresaRH on EmpresaRH.Id = RecursoH.EmpresaId
         LEFT JOIN Empresas EmpresaRHRef on EmpresaRH.EmpresaRefId = EmpresaRHRef.Id
         LEFT JOIN ProjetoEtapas Etapa on Registro.EtapaId = Etapa.Id
";

        public static string Down = "";
    }
}