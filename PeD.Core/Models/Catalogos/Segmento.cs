﻿using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Models.Catalogos
{
    public class Segmento
    {
        [Key] public string Valor { get; set; }
        public string Nome { get; set; }


        public override string ToString()
        {
            return Nome;
        }
    }
}