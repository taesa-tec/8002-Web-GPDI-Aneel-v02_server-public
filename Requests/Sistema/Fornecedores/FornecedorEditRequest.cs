namespace APIGestor.Requests.Sistema.Fornecedores
{
    public class FornecedorEditRequest : FornecedorCreateRequest
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public bool TrocarResponsavel { get; set; }
    }
}