using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoSelecaoPendenteDto : BaseEntity
    {
        public string Titulo { get; set; }
        public int PropostasRecebidas { get; set; }
    }
}