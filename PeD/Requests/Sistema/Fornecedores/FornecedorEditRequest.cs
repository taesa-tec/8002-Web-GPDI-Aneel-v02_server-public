namespace PeD.Requests.Sistema.Fornecedores
{
    public class FornecedorEditRequest : FornecedorCreateRequest
    {
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public bool TrocarResponsavel { get; set; }
    }
}