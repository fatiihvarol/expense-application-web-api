namespace Web.Api.Data.Entities
{
    public class VpRefreshToken
    {

        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public int UserId { get; set; }
        public VpApplicationUser User { get; set; } 
    }
}
