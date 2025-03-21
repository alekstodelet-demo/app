using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetRepresentativeContactResponse
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("r_type_id")]
        public int? RTypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }


        internal static GetRepresentativeContactResponse FromDomain(RepresentativeContact domain)
        {
            return new GetRepresentativeContactResponse
            {
                Id = domain.Id,
                Value = domain.Value,
                AllowNotification = domain.AllowNotification,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                RTypeId = domain.RTypeId,
                RepresentativeId = domain.RepresentativeId,

            };
        }
    }

    public class CreateRepresentativeContactRequest
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("r_type_id")]
        public int? RTypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }


        internal RepresentativeContact ToDomain()
        {
            return new RepresentativeContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                RTypeId = RTypeId,
                RepresentativeId = RepresentativeId,

            };
        }
    }

    public class UpdateRepresentativeContactRequest
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("r_type_id")]
        public int? RTypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }


        internal RepresentativeContact ToDomain()
        {
            return new RepresentativeContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                RTypeId = RTypeId,
                RepresentativeId = RepresentativeId,

            };
        }
    }
}
