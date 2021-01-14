using System;
using System.Collections.Generic;
using TaesaCore.Models;

namespace PeD.Core.Requests.Captacao
{
    public class ConfiguracaoRequest : BaseEntity
    {
        public string Consideracoes { get; set; }

        /// <summary>
        /// Data máxima para o envio da proposta
        /// </summary>
        public DateTime Termino { get; set; }

        /// <summary>
        /// Ids dos contratos
        /// </summary>
        public List<int> Contratos { get; set; }

        /// <summary>
        /// Ids dos arquivos que devem ser disponibilizados aos fornecedores
        /// </summary>
        public List<int> Arquivos { get; set; }

        /// <summary>
        /// Ids dos fornecedores que serão convidados
        /// </summary>
        public List<int> Fornecedores { get; set; }
    }
}