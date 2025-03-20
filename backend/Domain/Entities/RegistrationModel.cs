namespace Domain.Entities
{
    /// <summary>
    /// Запрос на регистрацию пользователя
    /// </summary>
    public class RegistrationRequest
    {
        public string EntityType { get; set; }
        public string Inn { get; set; }
        public string Password { get; set; }
        public string TaxCode { get; set; }
    }

    /// <summary>
    /// Результат регистрации пользователя
    /// </summary>
    public class RegistrationResult
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
        public string RedirectUrl { get; set; }
    }
}