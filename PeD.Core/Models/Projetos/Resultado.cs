using System.Collections.Generic;

namespace PeD.Core.Models.Projetos
{
    public class Resultado
    {
        public string Acao { get; set; }

        public bool Sucesso
        {
            get { return Inconsistencias == null || Inconsistencias.Count == 0; }
        }
        public string Id { get; set; }
        public List<string> Inconsistencias { get; } = new List<string>();
    }
}