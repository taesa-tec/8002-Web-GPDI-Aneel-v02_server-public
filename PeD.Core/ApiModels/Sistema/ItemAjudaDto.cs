namespace PeD.Core.ApiModels.Sistema
{
    public class ItemAjudaDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool HasContent { get; set; }
    }
}