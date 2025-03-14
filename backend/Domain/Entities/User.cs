namespace Domain.Entities;

public class User : BaseLogDomain
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string TwoFactorSecret { get; set; }
}

public class UserModel : BaseLogDomain
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string TwoFactorSecret { get; set; }
}