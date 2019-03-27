
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class AtividadesGestao
    {
        private int _id;

        [Key]
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public int ProjetoId { get; set; }
        public string DedicacaoHorario { get;set; }
        public string ResDedicacaoHorario { get;set; }
        public string ParticipacaoMembros { get; set; }
        public string ResParticipacaoMembros { get; set; }
        public string DesenvFerramenta { get;set; }
        public string ResDesenvFerramenta { get;set; }
        public string ProspTecnologica { get;set; }
        public string ResProspTecnologica { get;set; }
        public string DivulgacaoResultados { get;set; }
        public string ResDivulgacaoResultados { get;set; }
        public string ParticipacaoTecnicos { get;set; }
        public string ResParticipacaoTecnicos { get;set; }
        public string BuscaAnterioridade { get;set; }
        public string ResBuscaAnterioridade { get;set; }
        public string ContratacaoAuditoria { get;set; }
        public string ResContratacaoAuditoria { get;set; }
        public string ApoioCitenel { get;set; }
        public string ResApoioCitenel { get;set; }
    }
}