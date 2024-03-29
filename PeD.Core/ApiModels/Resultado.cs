﻿using System.Collections.Generic;

namespace PeD.Core.ApiModels
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