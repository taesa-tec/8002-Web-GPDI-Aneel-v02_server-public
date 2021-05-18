using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoIdentificaoRiscosDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Fornecedor { get; set; }
        public string IdentificacaoRiscoResponsavel { get; set; }
        public string AprovacaoResponsavel { get; set; }
    }
}