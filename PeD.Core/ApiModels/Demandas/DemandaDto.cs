using System;
using System.Collections.Generic;
using PeD.Core.Models.Demandas;

namespace PeD.Core.ApiModels.Demandas
{
    public class DemandaDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string CriadorId { get; set; }
        public string Criador { get; set; }
        public string SuperiorDiretoId { get; set; }
        public string SuperiorDireto { get; set; }

        public string RevisorId { get; set; }
        public string Revisor { get; set; }
        public DemandaEtapa EtapaAtual { get; set; }
        public DemandaStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CaptacaoDate { get; set; }
        public List<DemandaComentarioDto> Comentarios { get; set; }
    }
}