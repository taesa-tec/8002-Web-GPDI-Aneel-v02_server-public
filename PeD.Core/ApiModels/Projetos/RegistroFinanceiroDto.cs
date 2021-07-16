using System;

namespace PeD.Core.ApiModels.Projetos
{
    public class RegistroFinanceiroDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public string Status { get; set; }

        public int? ComprovanteId { get; set; }

        public int FinanciadoraId { get; set; }

        public string Financiadora { get; set; }
        public DateTime MesReferencia { get; set; }

        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public int EtapaId { get; set; }


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

        #endregion
    }
}