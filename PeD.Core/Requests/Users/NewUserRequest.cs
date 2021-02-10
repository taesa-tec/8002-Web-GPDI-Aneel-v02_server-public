namespace PeD.Core.Requests.Users
{
    public class NewUserRequest
    {
        public string Email { get; set; }

        public string NomeCompleto { get; set; }

        public string Cargo { get; set; }

        public string Cpf { get; set; }

        public bool Status { get; set; }

        public string Role { get; set; }

        public int? EmpresaId { get; set; }

        public string RazaoSocial { get; set; }
    }
}