using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetRepresentativeResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("second_name")]
        public string SecondName { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("has_access")]
        public bool? HasAccess { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }


        internal static GetRepresentativeResponse FromDomain(Representative domain)
        {
            return new GetRepresentativeResponse
            {
                Id = domain.Id,
                FirstName = domain.FirstName,
                SecondName = domain.SecondName,
                Pin = domain.Pin,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                CompanyId = domain.CompanyId,
                HasAccess = domain.HasAccess,
                TypeId = domain.TypeId,
                LastName = domain.LastName,

            };
        }
    }

    public class CreateRepresentativeRequest
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("second_name")]
        public string SecondName { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("has_access")]
        public bool? HasAccess { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }


        internal Representative ToDomain()
        {
            return new Representative
            {
                Id = Id,
                FirstName = FirstName,
                SecondName = SecondName,
                Pin = Pin,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                CompanyId = CompanyId,
                HasAccess = HasAccess,
                TypeId = TypeId,
                LastName = LastName,

            };
        }
    }

    public class UpdateRepresentativeRequest
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("second_name")]
        public string SecondName { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("company_id")]
        public int CompanyId { get; set; }

        [JsonProperty("has_access")]
        public bool? HasAccess { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }


        internal Representative ToDomain()
        {
            return new Representative
            {
                Id = Id,
                FirstName = FirstName,
                SecondName = SecondName,
                Pin = Pin,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                CompanyId = CompanyId,
                HasAccess = HasAccess,
                TypeId = TypeId,
                LastName = LastName,

            };
        }
    }
}
