using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoFormalizacaoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Fornecedor { get; set; }
        public string ExecucaoResponsavel { get; set; }
        public string AprovacaoResponsavel { get; set; }
    }
}