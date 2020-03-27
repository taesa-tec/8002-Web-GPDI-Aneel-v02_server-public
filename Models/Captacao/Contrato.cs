namespace APIGestor.Models.Captacao
{
    public class Contrato : BaseEntity
    {
        public enum TipoContrato
        {
            Executor,
            CoExecutor
        }

        public string Titulo { get; set; }
        public string Conteudo { get; set; }
        public TipoContrato Tipo { get; set; }
    }
}