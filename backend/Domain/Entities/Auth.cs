
namespace Domain.Entities
{
    public class AuthToken
    {
        public string token { get; set; }
    }

    public class UserInfo
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Id { get; set; }
    }
}
