using System;
using System.Linq;
using System.Collections.Generic;
using APIGestor.Models;

namespace APIGestor.Core.Equipe
{

    public class EquipePeD
    {
        public string Diretor { get; set; }
        public string Gerente { get; set; }
        public string Coordenador { get; set; }

        public List<string> Outros { get; set; }
        public EquipePeD() { }


    }
}