using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Projetos
{
    public class Alocacao : ProjetoNode
    {
        public string Tipo { get; set; }
        public int EtapaId { get; set; }
        public Etapa Etapa { get; set; }

        public int EmpresaFinanciadoraId { get; set; }
        public Empresa EmpresaFinanciadora { get; set; }

        public string Justificativa { get; set; }
        [NotMapped] public virtual decimal Custo { get; }
    }
}