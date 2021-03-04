using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaEscopos")]
    public class Escopo : PropostaNode
    {
        public string Objetivo { get; set; }
        public string ResultadoEsperado { get; set; }

        public string BeneficioTaesa { get; set; }
        public string BeneficioInstitucional { get; set; }
        public string BeneficioIndustria { get; set; }
        public string BeneficioSetorEletrico { get; set; }
        public string BeneficioSociedade { get; set; }

        public string ExperienciaPrevia { get; set; }
        public string Contrapartidas { get; set; }
    }
}