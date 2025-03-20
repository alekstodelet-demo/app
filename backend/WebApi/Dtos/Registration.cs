using Newtonsoft.Json;
using Domain.Entities;

namespace WebApi.Dtos
{
    public class CheckInnRequest
    {
        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("entityType")]
        public string EntityType { get; set; }
    }

    public class EntityInfoResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        internal static EntityInfoResponse FromDomain(EntityInfo domain)
        {
            if (domain == null) return null;

            return new EntityInfoResponse
            {
                Name = domain.Name,
                FullName = domain.FullName,
                TaxCode = domain.TaxCode,
                Address = domain.Address,
                PhoneNumber = domain.PhoneNumber
            };
        }
    }
    public class RegisterRequest
    {
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        [JsonProperty("inn")]
        public string Inn { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }

        internal RegistrationRequest ToDomain()
        {
            return new RegistrationRequest
            {
                EntityType = EntityType,
                Inn = Inn,
                Password = Password,
                TaxCode = TaxCode
            };
        }
    }

    public class RegisterResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        internal static RegisterResponse FromDomain(RegistrationResult domain)
        {
            return new RegisterResponse
            {
                UserId = domain.UserId,
                Token = domain.Token,
                ExpiresIn = domain.ExpiresIn,
                RedirectUrl = domain.RedirectUrl
            };
        }
    }


    public class CheckInnResponse
    {
        [JsonProperty("exists")]
        public bool Exists { get; set; }

        [JsonProperty("entityInfo")]
        public EntityInfoResponse EntityInfo { get; set; }

        internal static CheckInnResponse FromDomain(InnCheckResult domain)
        {
            return new CheckInnResponse
            {
                Exists = domain.Exists,
                EntityInfo = EntityInfoResponse.FromDomain(domain.EntityInfo)
            };
        }
    }

    public class ApiSuccessResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ApiErrorResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; } = false;

        [JsonProperty("error")]
        public ErrorDetails Error { get; set; }

        public class ErrorDetails
        {
            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }
    }
    public class ApiErrorResponseWithDetails<T> : ApiErrorResponse
    {
        [JsonProperty("details")]
        public T Details { get; set; }
    }
}