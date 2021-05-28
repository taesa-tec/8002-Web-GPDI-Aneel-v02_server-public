using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class ProjetoEscopoDto
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
}