using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetRepresentativeContactResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }

        internal static GetRepresentativeContactResponse FromDomain(RepresentativeContact domain)
        {
            return new GetRepresentativeContactResponse
            {
                Id = domain.Id,
                Value = domain.Value,
                AllowNotification = domain.AllowNotification,
                TypeId = domain.TypeId,
                RepresentativeId = domain.RepresentativeId
            };
        }
    }

    public class CreateRepresentativeContactRequest
    {
        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }

        internal RepresentativeContact ToDomain()
        {
            return new RepresentativeContact
            {
                Value = Value,
                AllowNotification = AllowNotification,
                TypeId = TypeId,
                RepresentativeId = RepresentativeId
            };
        }
    }

    public class UpdateRepresentativeContactRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }

        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("type_id")]
        public int TypeId { get; set; }

        [JsonProperty("representative_id")]
        public int RepresentativeId { get; set; }

        internal RepresentativeContact ToDomain()
        {
            return new RepresentativeContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                TypeId = TypeId,
                RepresentativeId = RepresentativeId
            };
        }
    }
}