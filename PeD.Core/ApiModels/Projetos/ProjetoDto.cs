using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PeD.Core.ApiModels.Captacao;
using PeD.Core.Converters;
using PeD.Core.Models.Projetos;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Projetos
{
    public class ProjetoDto : BaseEntity
    {
        public string ResponsavelId { get; set; }
        public string Titulo { get; set; }
        public string TituloCompleto { get; set; }
        public string Status { get; set; }
        public int PropostaId { get; set; }
        public string Codigo { get; set; }
        public string Numero { get; set; }
        public int? ProponenteId { get; set; }
        public string Proponente { get; set; }

        public int PlanoTrabalhoFileId { get; set; }

        public int ContratoId { get; set; }

        public int EspecificacaoTecnicaFileId { get; set; }

        public int? TemaId { get; set; }
        public string Tema { get; set; }

        public string Captacao { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int CaptacaoId { get; set; }
        public short Duracao { get; set; }
        public bool ContratoFinalizado { get; set; }
        public bool PlanoFinalizado { get; set; }

        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        [JsonConverter(typeof(DateConverter))] public DateTime DataInicioProjeto { get; set; }
        [JsonConverter(typeof(DateConverter))] public DateTime DataFinalProjeto { get; set; }
    }

    public abstract class ProjetoNodeDto : BaseEntity
    {
        public int ProjetoId { get; set; }
    }
}