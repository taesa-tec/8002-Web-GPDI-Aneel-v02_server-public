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

    public class DemandaFileDto : FileUploadDto
    {
    }

    public class DemandaFormFileDto
    {
        public int Id { get; set; }
        public int DemandaFormId { get; set; }
        public int FileId { get; set; }
        public DemandaFileDto File { get; set; }
    }

    public class DemandaLogDto : LogDto
    {
        public int DemandaId { get; set; }
        public DemandaDto Demanda { get; set; }
    }
}