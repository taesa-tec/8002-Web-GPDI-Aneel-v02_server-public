using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PeD.Core.Models.Projetos
{
    [Table("ProjetoRecursosHumanos")]
    public class RecursoHumano : ProjetoNode
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        [Column(TypeName = "decimal(10, 2)")] public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }

        public int? CoExecutorId { get; set; }
        public CoExecutor CoExecutor { get; set; }

        public class AlocacaoRh : Alocacao
        {
            public int RecursoHumanoId { get; set; }
            public RecursoHumano RecursoHumano { get; set; }
            public List<AlocacaoRhHorasMes> HorasMeses { get; set; }

            public override decimal Custo
            {
                get
                {
                    return (HorasMeses is null) ? 0 : HorasMeses.Sum(i => i.Horas) * (RecursoHumano?.ValorHora ?? 0);
                }
            }
        }

        public class AlocacaoRhHorasMes
        {
            public int AlocacaoRhId { get; set; }
            public int Mes { get; set; }
            public int Horas { get; set; }
        }
    }
}