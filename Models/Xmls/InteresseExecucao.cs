
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class InteresseExecucao
    {
        public PD_InteresseProjeto PD_InteresseProjeto { get; set; }
    }
    public class PD_InteresseProjeto
    {
        public InteresseProjeto Projeto{ get; set; }
    }
    public class InteresseProjeto
    {
        public string CodProjeto { get; set; }
        private string _interesse{ get; set; }
        public string Interesse{
            get => "S";
            set => _interesse = value;
        }
    }
}