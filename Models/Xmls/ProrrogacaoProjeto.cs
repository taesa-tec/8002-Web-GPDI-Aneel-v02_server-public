
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIGestor.Models
{
    public class ProrrogacaoProjeto
    {
        public PD_PrazoExecProjeto PD_PrazoExecProjeto { get; set; }
    }
    public class PD_PrazoExecProjeto
    {
        public ProProjeto Projeto { get; set; }
    }
    public class ProProjeto
    {
        public string CodProjeto { get; set; }
        public int Duracao { get; set; }
    }
}