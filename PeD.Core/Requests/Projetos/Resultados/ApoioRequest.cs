using System.ComponentModel.DataAnnotations;
using TaesaCore.Models;

namespace PeD.Core.Requests.Projetos.Resultados
{
    public class ApoioRequest : BaseEntity
    {
        public string Tipo { get; set; }

        public string CnpjReceptora { get; set; }
        [MaxLength(100)] public string Laboratorio { get; set; }
        [MaxLength(50)] public string LaboratorioArea { get; set; }
        [MaxLength(300)] public string MateriaisEquipamentos { get; set; }
    }
}