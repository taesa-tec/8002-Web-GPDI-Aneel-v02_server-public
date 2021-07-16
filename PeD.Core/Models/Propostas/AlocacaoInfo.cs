using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    public class AlocacaoInfo
    {
        public string Categoria { get; set; }
        public int PropostaId { get; set; }
        public int EtapaId { get; set; }
        public short EtapaOrdem { get; set; }
        public int EmpresaFinanciadoraId { get; set; }
        public string EmpresaFinanciadora { get; set; }
        public Funcao EmpresaFinanciadoraFuncao { get; set; }
        public string Justificativa { get; set; }
        public int RecursoId { get; set; }
        public string Recurso { get; set; }
        public int EmpresaRecebedoraId { get; set; }
        public string EmpresaRecebedora { get; set; }
        public Funcao EmpresaRecebedoraFuncao { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Quantidade { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Valor { get; set; }
        [Column(TypeName = "decimal(18,2)")] public decimal Custo { get; set; }
    }
}