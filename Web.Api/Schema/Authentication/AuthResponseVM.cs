namespace Web.Api.Schema.Authentication
{
    public class AuthResponseVM
    {

        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
