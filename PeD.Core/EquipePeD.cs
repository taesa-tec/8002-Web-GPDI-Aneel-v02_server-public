using System.Collections.Generic;

namespace PeD.Core
{
    public class EquipePeD
    {
        public string Diretor { get; set; }
        public string Gerente { get; set; }
        public string Coordenador { get; set; }

        public List<string> Outros { get; set; }

        public List<string> CargosChavesIds
        {
            get { return new List<string> {Diretor, Gerente, Coordenador}; }
        }
    }
}