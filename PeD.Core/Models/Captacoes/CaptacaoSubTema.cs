using System.ComponentModel.DataAnnotations.Schema;
using PeD.Core.Models.Catalogos;
using TaesaCore.Models;

namespace PeD.Core.Models.Captacoes
{
    [Table("CaptacaoSubTemas")]
    public class CaptacaoSubTema : BaseEntity
    {
        public string Outro { get; set; }
        public int CaptacaoId { get; set; }
        public Captacao Captacao { get; set; }

        public int? SubTemaId { get; set; }
        public Tema SubTema { get; set; }
    }
}