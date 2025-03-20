namespace Domain.Entities
{
    /// <summary>
    /// Запрос на регистрацию международной организации
    /// </summary>
    public class InternationalRegistrationRequest
    {
        public string OrganizationName { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryCode { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Результат регистрации международной организации
    /// </summary>
    public class InternationalRegistrationResult
    {
        public string OrganizationId { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string RedirectUrl { get; set; }
        public string Status { get; set; }
    }
}