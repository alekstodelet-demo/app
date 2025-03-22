namespace Domain.Entities
{
    public class RefreshToken : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public bool IsUsed { get; set; }
        public string UserId { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string DeviceId { get; set; }
        public DateTime? RevokedDate { get; set; }
        public string ReplacedByToken { get; set; }
        public string RevokedReason { get; set; }
    }
}