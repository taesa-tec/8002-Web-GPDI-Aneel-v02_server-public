using System;
using System.Collections.Generic;
using PeD.Core.ApiModels.Fornecedores;
using TaesaCore.Models;

namespace PeD.Core.ApiModels.Captacao
{
    public class CaptacaoDetalhesDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Status { get; set; }
        public string EspecificacaoTecnicaUrl { get; set; }
        public string Observacoes { get; set; }
        public string Consideracoes { get; set; }
        public DateTime? Termino { get; set; }
        public int? ContratoSugeridoId { get; set; }
        public string ContratoSugerido { get; set; }

        public int? ContratoId { get; set; }
        public string Contrato { get; set; }
        public int DemandaId { get; set; }
        public bool Finalizada => Termino < DateTime.Today;

        public List<FornecedorDto> FornecedoresSugeridos { get; set; }
        public List<FornecedorDto> FornecedoresConvidados { get; set; }
        public List<CaptacaoArquivoDto> Arquivos { get; set; }
    }
}