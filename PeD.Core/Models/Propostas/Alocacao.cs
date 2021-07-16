namespace PeD.Core.Models.Propostas
{
    public abstract class Alocacao : PropostaNode
    {
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }

        public int EmpresaFinanciadoraId { get; set; }
        public Empresa EmpresaFinanciadora { get; set; }

        public string Justificativa { get; set; }
        public abstract decimal Valor { get; }
    }
}