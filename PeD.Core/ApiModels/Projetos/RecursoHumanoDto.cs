namespace PeD.Core.ApiModels.Projetos
{
    public class RecursoHumanoDto : ProjetoNodeDto
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public int? EmpresaId { get; set; }
        public int? CoExecutorId { get; set; }
        public string Empresa { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }
    }

    public class RegistroFinanceiroDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string Status { get; set; }


        public int? FinanciadoraId { get; set; }
        public int? CoExecutorFinanciadorId { get; set; }

        public string Financiadora { get; set; }


        #region Recurso Humano

        public string AtividadeRealizada { get; set; }
        public int RecursoHumanoId { get; set; }
        public string RecursoHumano { get; set; }
        public decimal Horas { get; set; }

        #endregion

        #region Recurso Material

        public string NomeItem { get; set; }

        public string Beneficiado { get; set; }

        public int RecursoMaterialId { get; set; }
        public string RecursoMaterial { get; set; }

        public string CnpjBeneficiado { get; set; }

        public int CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }
        public bool EquipaLaboratorioExistente { get; set; }
        public bool EquipaLaboratorioNovo { get; set; }
        public bool IsNacional { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string FuncaoEtapa { get; set; }

        public int? RecebedoraId { get; set; }
        public string Recebedora { get; set; }

        public int? CoExecutorRecebedorId { get; set; }
        public string CoExecutorRecebedor { get; set; }

        #endregion
    }
}