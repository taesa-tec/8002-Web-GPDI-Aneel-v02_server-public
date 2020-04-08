using System.Collections.Generic;
using APIGestor.Models.Captacao;
using TaesaCore.Models;

namespace APIGestor.Dtos.Captacao
{
    public class CaptacaoDetalhesDto : BaseEntity
    {
        public string EspecificacaoTecnicaUrl { get; set; }
        public List<CaptacaoArquivo> Arquivos { get; set; }
        public List<Models.Fornecedores.Fornecedor> SugestaoFornecedores { get; set; }
        public string Observacoes { get; set; }
    }
}