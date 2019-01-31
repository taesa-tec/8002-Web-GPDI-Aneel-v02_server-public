namespace APIGestor.Security
{
    public class Login
    {
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class User
    {
        public string UserID { get; set; }
        public string NewPassword { get; set; }
        public string ResetToken { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string NomeCompleto { get; set; }
    }

    public static class Roles
    {
        public const string ROLE_ADMIN_GESTOR = "Admin-APIGestor";
        public const string ROLE_USER_GESTOR = "User-APIGestor";
    }

    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }

    public class Token
    {
        public bool Authenticated { get; set; }
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public string Message { get; set; }
    }
}