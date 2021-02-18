using System;
using System.Collections.Generic;
using PeD.Core.ApiModels.Fornecedores;
using PeD.Core.Models.Propostas;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Propostas
{
    public class ContratoListItemDto : PropostaNodeDto
    {
        public int ParentId { get; set; }
        public string Titulo { get; set; }
        public bool Finalizado { get; set; }
    }

    public class ContratoDto : PropostaNodeDto
    {
        public int ParentId { get; set; }
        public Captacao.ContratoDto Parent { get; set; }
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public List<ContratoRevisaoDto> Revisoes { get; set; }
        public bool Finalizado { get; set; }
    }

    public class ContratoRevisaoDto : PropostaNodeDto
    {
        public int ParentId { get; set; }
        public string Conteudo { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ContratoRevisaoListItemDto : BaseEntityDto
    {
        public DateTime CreatedAt { get; set; }
    }
}