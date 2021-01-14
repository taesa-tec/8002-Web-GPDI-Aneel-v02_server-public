namespace PeD.Auth
{
    public class User
    {
        public string UserID { get; set; }
        public string NewPassword { get; set; }
        public string ResetToken { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string NomeCompleto { get; set; }
    }
}