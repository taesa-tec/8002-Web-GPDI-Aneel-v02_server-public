using System.ComponentModel.DataAnnotations.Schema;

namespace PeD.Core.Models.Propostas
{
    [Table("PropostaRecursosHumanos")]
    public class RecursoHumano : PropostaNode
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public Empresa Empresa { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }

        public int? CoExecutorId { get; set; }
        public CoExecutor CoExecutor { get; set; }

        public class Alocacao : Propostas.Alocacao
        {
            public int RecursoId { get; set; }

            public RecursoHumano Recurso { get; set; }
            // @todo Horas Alocadas
        }
    }
}