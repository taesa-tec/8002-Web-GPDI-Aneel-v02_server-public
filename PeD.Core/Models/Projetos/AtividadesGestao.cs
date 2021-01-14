using System.ComponentModel.DataAnnotations;
using PeD.Core.Attributes;

namespace PeD.Core.Models.Projetos {
    public class AtividadesGestao {
        private int _id;

        [Key]
        public int Id {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        [Logger("Dedicação Horária")]
        public string DedicacaoHorario { get; set; }
        [Logger("Resposta Dedicação Horária")]
        public string ResDedicacaoHorario { get; set; }
        [Logger("Participação dos Membros")]
        public string ParticipacaoMembros { get; set; }
        [Logger("Resposta Participação dos Membros")]
        public string ResParticipacaoMembros { get; set; }
        [Logger("Desenvolvimento de ferramenta")]
        public string DesenvFerramenta { get; set; }
        [Logger("Resposta Desenvolvimento de ferramenta")]
        public string ResDesenvFerramenta { get; set; }
        [Logger("Prospecção tecnológica")]
        public string ProspTecnologica { get; set; }
        [Logger("Resposta Prospecção tecnológica")]
        public string ResProspTecnologica { get; set; }
        [Logger("Divulgação de resultados")]
        public string DivulgacaoResultados { get; set; }
        [Logger("Resposta Divulgação de resultados")]
        public string ResDivulgacaoResultados { get; set; }
        [Logger("Participação dos responsáveis técnico")]
        public string ParticipacaoTecnicos { get; set; }
        [Logger("Resposta Participação dos responsaveis técnicos")]
        public string ResParticipacaoTecnicos { get; set; }
        [Logger("Busca de anterioridade")]
        public string BuscaAnterioridade { get; set; }
        [Logger("Resposta Busca de anteriordade")]
        public string ResBuscaAnterioridade { get; set; }
        [Logger("Contratação de auditoria")]
        public string ContratacaoAuditoria { get; set; }
        [Logger("Resposta Contratação de auditoria")]
        public string ResContratacaoAuditoria { get; set; }
        [Logger("Apoio Citenel")]
        public string ApoioCitenel { get; set; }
        [Logger("Resposta Apoio Citenel")]
        public string ResApoioCitenel { get; set; }
    }
}