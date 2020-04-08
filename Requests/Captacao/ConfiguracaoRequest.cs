using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace APIGestor.Requests.Captacao
{
    public class ConfiguracaoRequest : BaseEntity
    {
        public string Consideracoes { get; set; }
        public DateTime Termino { get; set; }
        public List<int> Contratos { get; set; }
        public List<int> Arquivos { get; set; }
        public List<int> Convidados { get; set; }
    }
}