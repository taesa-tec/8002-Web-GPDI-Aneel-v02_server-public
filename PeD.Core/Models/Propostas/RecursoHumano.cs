using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRecursosHumanos")]
    public class RecursoHumano : PropostaNode
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        [Column(TypeName = "decimal(10, 2)")] public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }


        [Table("PropostaRecursosHumanosAlocacao")]
        public class AlocacaoRh : Propostas.Alocacao
        {
            public int RecursoId { get; set; }
            public RecursoHumano Recurso { get; set; }
            public Dictionary<short, short> HoraMeses { get; set; }

            public override decimal Valor
            {
                get { return HoraMeses.Sum(i => i.Value) * (Recurso?.ValorHora ?? 0); }
            }
        }
    }
}