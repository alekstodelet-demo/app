using Newtonsoft.Json;
using Domain.Entities;

namespace WebApi.Dtos
{
    public class RegisterInternationalRequest
    {
        [JsonProperty("organizationName")]
        public string OrganizationName { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("contactPerson")]
        public string ContactPerson { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        internal InternationalRegistrationRequest ToDomain()
        {
            return new InternationalRegistrationRequest
            {
                OrganizationName = OrganizationName,
                RegistrationNumber = RegistrationNumber,
                CountryCode = CountryCode,
                Address = Address,
                ContactPerson = ContactPerson,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Password = Password
            };
        }
    }

    public class RegisterInternationalResponse
    {
        [JsonProperty("organizationId")]
        public string OrganizationId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        internal static RegisterInternationalResponse FromDomain(InternationalRegistrationResult domain)
        {
            return new RegisterInternationalResponse
            {
                OrganizationId = domain.OrganizationId,
                Token = domain.Token,
                ExpiresIn = domain.ExpiresIn,
                RedirectUrl = domain.RedirectUrl,
                Status = domain.Status
            };
        }
    }
}