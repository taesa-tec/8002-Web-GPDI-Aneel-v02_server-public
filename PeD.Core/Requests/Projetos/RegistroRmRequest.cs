using System;

namespace PeD.Core.Requests.Projetos
{
    public class RegistroRmRequest
    {
        public string NomeItem { get; set; }
        public int EtapaId { get; set; }
        public int? ComprovanteId { get; set; }
        public int RecursoMaterialId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public int RecebedoraId { get; set; }
        public int FinanciadoraId { get; set; }
        public DateTime MesReferencia { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataDocumento { get; set; }
        public string Beneficiado { get; set; }
        public string CnpjBeneficiado { get; set; }
        public int CategoriaContabilId { get; set; }
        public bool EquipaLaboratorioExistente { get; set; }
        public bool EquipaLaboratorioNovo { get; set; }
        public bool IsNacional { get; set; }
        public string FuncaoEtapa { get; set; }
        public string ObservacaoInterna { get; set; }
        public string EspecificaoTecnica { get; set; }
    }
}