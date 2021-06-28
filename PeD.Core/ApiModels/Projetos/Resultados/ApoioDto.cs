namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class ApoioDto : ProjetoNodeDto
    {
        public enum TipoEstrutura
        {
            LNS,
            LES,
            LNP,
            LEP,
            LNE,
            LEE
        }

        public TipoEstrutura Tipo { get; set; }

        public string CnpjReceptora { get; set; }
         public string Laboratorio { get; set; }
         public string LaboratorioArea { get; set; }
         public string MateriaisEquipamentos { get; set; }
    }
}