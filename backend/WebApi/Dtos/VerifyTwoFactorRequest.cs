using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class VerifyTwoFactorRequest
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Code must contain exactly 6 digits")]
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class ValidateTwoFactorRequest
    {
        [Required]
        [EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Code must be 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Code must contain exactly 6 digits")]
        [JsonProperty("code")]
        public string Code { get; set; }
    }

    public class TwoFactorSetupResponse
    {
        [JsonProperty("secretKey")]
        public string SecretKey { get; set; }

        [JsonProperty("qrCodeUri")]
        public string QrCodeUri { get; set; }
    }
}