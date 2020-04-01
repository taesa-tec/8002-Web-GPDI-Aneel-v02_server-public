namespace APIGestor.Requests.Sistema.Fornecedores
{
    public class FornecedorCreateRequest
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Responsavel { get; set; }
        public string EmailResponsavel { get; set; }
    }
}