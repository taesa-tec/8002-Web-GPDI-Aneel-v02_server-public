using System;
using PeD.Core.ApiModels.Demandas;

namespace PeD.Core.ApiModels
{
    public class FileUploadDto : BaseEntityDto
    {
        public string UserId { get; set; }
        public ApplicationUserDto User { get; set; }

        public long Size { get; set; }

        /// <summary>
        /// Real filename
        /// </summary>
        public string FileName { get; set; }

        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }

        public int DemandaId { get; set; }
        public DemandaDto Demanda { get; set; }
    }
}