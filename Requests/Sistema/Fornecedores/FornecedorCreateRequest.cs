namespace APIGestor.Requests.Sistema.Fornecedores
{
    public class FornecedorCreateRequest
    {
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelEmail { get; set; }
    }
}