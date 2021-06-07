using System;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class RegistroFinanceiroInfoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int ProjetoId { get; set; }
        public DateTime MesReferencia { get; set; }
        
        public string AtividadeRealizada { get; set; }
        public string EspecificaoTecnica { get; set; }
        public string FuncaoEtapa { get; set; }
        
        public string NomeItem { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public bool? EquipaLaboratorioExistente { get; set; }
        public bool? EquipaLaboratorioNovo { get; set; }
        public bool? IsNacional { get; set; }
        public string Status { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public string Recurso { get; set; }
        public int? RecursoHumanoId { get; set; }
        public int? RecursoMaterialId { get; set; }
        public int? FinanciadoraId { get; set; }
        public int? CoExecutorFinanciadorId { get; set; }
        public int? RecebedoraId { get; set; }
        public int? CoExecutorRecebedorId { get; set; }
        public int? CategoriaContabilId { get; set; }
        public string CategoriaContabil { get; set; }
        public string Financiador { get; set; }
        public string Recebedor { get; set; }
        public decimal Valor { get; set; }
        public decimal Custo { get; set; }
        public decimal QuantidadeHoras { get; set; }
    }

    public class RegistroObservacaoDto : BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public int RegistroId { get; set; }

        public string AuthorId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}