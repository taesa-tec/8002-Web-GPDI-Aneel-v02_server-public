namespace PeD.Core.ApiModels.Auth
{
    public class Token
    {
        public string AccessToken { get; set; }
        public ApplicationUserDto User { get; set; }
    }
}