namespace PeD.Core.ApiModels.Projetos.Resultados
{
    public class ApoioDto : ProjetoNodeDto
    {
        public string Tipo { get; set; }
        public string CnpjReceptora { get; set; }
        public string Laboratorio { get; set; }
        public string LaboratorioArea { get; set; }
        public string MateriaisEquipamentos { get; set; }
    }
}