using System;
using PeD.Core.Models;
using PeD.Core.Models.Catalogos;

namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class ProducaoCientificaDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool ConfirmacaoPublicacao { get; set; }
        public string NomeEventoPublicacao { get; set; }

        public string LinkPublicacao { get; set; }

        public int PaisId { get; set; }
        public string Pais { get; set; }
        public string Cidade { get; set; }
        public string TituloTrabalho { get; set; }

        public int? ArquivoTrabalhoOrigemId { get; set; }
    }
}