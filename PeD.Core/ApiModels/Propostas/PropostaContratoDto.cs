using System;
using System.Collections.Generic;

namespace PeD.Core.ApiModels.Propostas
{
    public class ContratoListItemDto : PropostaNodeDto
    {
        public int ParentId { get; set; }
        public string Titulo { get; set; }
        public bool Finalizado { get; set; }
    }

    public class PropostaContratoDto : PropostaNodeDto
    {
        public int ParentId { get; set; }
        public Captacao.ContratoDto Parent { get; set; }
        public string Titulo { get; set; }
        public string Rascunho { get; set; }
        public string Conteudo { get; set; }
        public List<ContratoRevisaoDto> Revisoes { get; set; }
        public bool Finalizado { get; set; }

        public string Header { get; set; }
        public string Footer { get; set; }
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