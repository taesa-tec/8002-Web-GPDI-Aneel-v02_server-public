using System;
using System.Collections.Generic;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class ProjetoDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string TituloCompleto { get; set; }
        public string Status { get; set; }
        public int PropostaId { get; set; }
        public string Codigo { get; set; }
        public string Numero { get; set; }
        public int? ProponenteId { get; set; }
        public string Proponente { get; set; }


        public string Captacao { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public short Duracao { get; set; }
        public bool ContratoFinalizado { get; set; }
        public bool PlanoFinalizado { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataInicioProjeto { get; set; }
        public DateTime DataFinalProjeto { get; set; }
    }

    public abstract class ProjetoNodeDto : BaseEntity
    {
        public int ProjetoId { get; set; }
    }
}