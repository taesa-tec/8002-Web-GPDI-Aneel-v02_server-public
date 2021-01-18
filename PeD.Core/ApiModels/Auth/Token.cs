namespace PeD.Core.ApiModels.Auth
{
    public class Token
    {
        public string Created { get; set; }
        public string Expiration { get; set; }
        public string AccessToken { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}