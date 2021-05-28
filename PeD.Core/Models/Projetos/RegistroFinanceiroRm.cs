using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public class RegistroFinanceiroRm : RegistroFinanceiro
    {
        public string NomeItem { get; set; }

        public string Beneficiado { get; set; }

        public int RecursoMaterialId { get; set; }
        public RecursoMaterial RecursoMaterial { get; set; }

        public string CnpjBeneficiado { get; set; }

        public int CategoriaContabilId { get; set; }
        public Catalogos.CategoriaContabil CategoriaContabil { get; set; }
        public bool EquipaLaboratorioExistente { get; set; }
        public bool EquipaLaboratorioNovo { get; set; }
        public bool IsNacional { get; set; }
        public int Quantidade { get; set; }
        [Column(TypeName = "decimal(18, 2)")] public decimal Valor { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string FuncaoEtapa { get; set; }


        #region Empresa Recebedora

        public int? RecebedoraId { get; set; }
        public Empresa Recebedora { get; set; }

        public int? CoExecutorRecebedorId { get; set; }
        public CoExecutor CoExecutorRecebedor { get; set; }

        #endregion
    }
}