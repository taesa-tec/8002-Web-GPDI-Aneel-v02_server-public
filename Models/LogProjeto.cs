using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using static APIGestor.Models.LogItem;
using System.Reflection;
using System.Collections;

namespace APIGestor.Models
{
    public class LogProjeto : Log
    {

        public int ProjetoId { get; set; }
        [ForeignKey("ProjetoId")]
        public Projeto Projeto { get; set; }
    }



}