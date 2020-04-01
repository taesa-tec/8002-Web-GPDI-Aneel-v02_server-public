using System.Linq;
using System.Collections.Generic;

namespace APIGestor.Core.Equipe
{

    public class EquipePeD
    {
        public string Diretor { get; set; }
        public string Gerente { get; set; }
        public string Coordenador { get; set; }

        public List<string> Outros { get; set; }
        public EquipePeD() { }

        public List<string> CargosChavesIds
        {
            get
            {
                return new List<string>() { Diretor, Gerente, Coordenador };
            }
        }

        public List<string> Ids
        {
            get
            {
                return CargosChavesIds.Concat(Outros).ToList();
            }
        }


    }
}