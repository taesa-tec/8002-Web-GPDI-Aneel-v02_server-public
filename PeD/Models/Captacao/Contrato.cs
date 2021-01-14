using TaesaCore.Models;

namespace PeD.Models.Captacao
{
    public class Contrato : BaseEntity
    {
        public enum TipoContrato
        {
            Executor,
            CoExecutor
        }

        public enum Status
        {
            Pendente,
            Rascunho,
            Finalizado
        }

        public string Titulo { get; set; }
        public string Header { get; set; }
        public string Conteudo { get; set; }
        public string Footer { get; set; }
    }
}