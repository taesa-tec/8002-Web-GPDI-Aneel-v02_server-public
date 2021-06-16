using System;

namespace PeD.Core.ApiModels.Projetos
{
    public class ProjetoXmlDto : ProjetoNodeDto
    {
        public int FileId { get; set; }
        public string File { get; set; }
        public string Versao { get; set; }
        public string Tipo { get; set; }
        public DateTime Data { get; set; }
    }
}