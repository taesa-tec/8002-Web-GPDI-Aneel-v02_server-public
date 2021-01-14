using System.Collections.Generic;
using PeD.Dtos.FornecedoresDtos;
using TaesaCore.Models;

namespace PeD.Dtos.Captacao
{
    public class CaptacaoDetalhesDto : BaseEntity
    {
        public string Titulo { get; set; }
        public string Status { get; set; }
        public string EspecificacaoTecnicaUrl { get; set; }
        public List<CaptacaoArquivoDto> Arquivos { get; set; }
        public List<FornecedorDto> FornecedoresSugeridos { get; set; }
        public string Observacoes { get; set; }
    }
}