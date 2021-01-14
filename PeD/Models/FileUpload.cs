using System;
using Newtonsoft.Json;

namespace PeD.Models
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

    public class FileUpload : IFileUpload
    {
        public FileUpload()
        {
        }

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
}