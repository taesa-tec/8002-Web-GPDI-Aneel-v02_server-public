using System;
using Newtonsoft.Json;
using TaesaCore.Models;

namespace PeD.Core.Models
{
    public interface IFileUpload
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long Size { get; set; }
        [JsonIgnore] public string Path { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class FileUpload : BaseEntity, IFileUpload
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public long Size { get; set; }
        [JsonIgnore] public string Path { get; set; }
        
        /// <summary>
        /// Name file form
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Real filename
        /// </summary>
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}