namespace PeD.Core.ApiModels.Propostas
{
    public class RecursoHumanoDto : PropostaNodeDto
    {
        public string NomeCompleto { get; set; }
        public string Titulacao { get; set; }
        public string Funcao { get; set; }
        public string Nacionalidade { get; set; }
        public int? EmpresaId { get; set; }
        public int? CoExecutorId { get; set; }
        public string Empresa { get; set; }

        //Cpf ou Passaport
        public string Documento { get; set; }

        public decimal ValorHora { get; set; }
        public string UrlCurriculo { get; set; }
    }
}