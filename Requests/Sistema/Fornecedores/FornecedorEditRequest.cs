namespace APIGestor.Requests.Sistema.Fornecedores
{
    public class FornecedorEditRequest : FornecedorCreateRequest
    {
        public bool Status { get; set; }
        public bool TrocarResponsavel { get; set; }
    }
}