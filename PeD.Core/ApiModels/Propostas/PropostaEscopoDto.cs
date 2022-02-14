using System.Collections.Generic;
using FluentValidation;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class PropostaEscopoDto
    {
        public class MetaDto : BaseEntity

        {
            public string Objetivo { get; set; }
            public short Meses { get; set; }
        }

        public string Objetivo { get; set; }
        public string ResultadoEsperado { get; set; }
        
        public string BeneficioTaesa { get; set; }
        public string BeneficioInstitucional { get; set; }
        public string BeneficioIndustria { get; set; }
        public string BeneficioSetorEletrico { get; set; }
        public string BeneficioSociedade { get; set; }

        public string ExperienciaPrevia { get; set; }
        public string Contrapartidas { get; set; }
        public List<MetaDto> Metas { get; set; }
    }
    public class PropostaEscopoDtoValidator : AbstractValidator<PropostaEscopoDto>
    {
        public PropostaEscopoDtoValidator()
        {
            RuleFor(r => r).NotNull();
            RuleFor(r => r.ResultadoEsperado).NotEmpty();
            RuleFor(r => r.Objetivo).NotEmpty();
            RuleFor(r => r.BeneficioTaesa).NotEmpty();
            RuleFor(r => r.BeneficioInstitucional).NotEmpty();
            RuleFor(r => r.BeneficioIndustria).NotEmpty();
            RuleFor(r => r.BeneficioSetorEletrico).NotEmpty();
            RuleFor(r => r.BeneficioSociedade).NotEmpty();
            RuleFor(r => r.ExperienciaPrevia).NotEmpty();
            RuleFor(r => r.Contrapartidas).NotEmpty();
            RuleFor(r => r.Metas).NotEmpty();
        }
    }
}