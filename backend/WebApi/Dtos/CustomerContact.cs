using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetCustomerContactResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("value")]
        public string? Value { get; set; }
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }


        internal static GetCustomerContactResponse FromDomain(CustomerContact domain)
        {
            return new GetCustomerContactResponse
            {
                Id = domain.Id,
                Value = domain.Value,
                AllowNotification = domain.AllowNotification,
                TypeName = domain.TypeName,
                TypeId = domain.TypeId,
            };
        }
    }

    public class CreateCustomerContactRequest
    {
        [JsonProperty("value")]
        public string? Value { get; set; }
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        internal CustomerContact ToDomain()
        {
            return new CustomerContact
            {
                Value = Value,
                AllowNotification = AllowNotification,
                TypeName = TypeName,
                TypeId = TypeId,
            };
        }
    }

    public class UpdateCustomerContactRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("value")]
        public string? Value { get; set; }
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        [JsonProperty("type_name")]
        public string TypeName { get; set; }
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }

        internal CustomerContact ToDomain()
        {
            return new CustomerContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                TypeName = TypeName,
                TypeId = TypeId,
            };
        }
    }
}
