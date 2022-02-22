using System;
using FluentValidation;

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

    public class RegistroRmRequestValidator : AbstractValidator<RegistroRmRequest>
    {
        public RegistroRmRequestValidator()
        {
            RuleFor(r => r.NomeItem).NotEmpty();
            RuleFor(r => r.EtapaId).NotNull();
            RuleFor(r => r.RecursoMaterialId).NotNull();
            RuleFor(r => r.Quantidade).GreaterThan(0);
            RuleFor(r => r.Valor).GreaterThan(0);
            RuleFor(r => r.RecebedoraId).NotNull();
            RuleFor(r => r.FinanciadoraId).NotNull();
            RuleFor(r => r.MesReferencia).NotEmpty();
            RuleFor(r => r.TipoDocumento).NotEmpty();
            RuleFor(r => r.NumeroDocumento).NotEmpty();
            RuleFor(r => r.DataDocumento).NotNull();
            RuleFor(r => r.Beneficiado).NotEmpty();
            RuleFor(r => r.CnpjBeneficiado).NotEmpty();
            RuleFor(r => r.CategoriaContabilId).NotNull();
            RuleFor(r => r.EquipaLaboratorioExistente).NotEmpty();
            RuleFor(r => r.EquipaLaboratorioNovo).NotEmpty();
            RuleFor(r => r.IsNacional).NotEmpty();
            RuleFor(r => r.FuncaoEtapa).NotEmpty();
            RuleFor(r => r.EspecificaoTecnica).NotEmpty();
        }
    }
}