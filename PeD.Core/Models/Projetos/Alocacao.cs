namespace PeD.Core.Models.Projetos
{
    public abstract class Alocacao : ProjetoNode
    {
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }
        public int? EmpresaFinanciadoraId { get; set; }
        public Empresa EmpresaFinanciadora { get; set; }

        public int? CoExecutorFinanciadorId { get; set; }
        public CoExecutor CoExecutorFinanciador { get; set; }

        public string Justificativa { get; set; }
        public abstract decimal Valor { get; }
    }
}