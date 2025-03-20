using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetOrganizationContactResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("value")]
        public string? Value { get; set; }
        
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }
        
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        
        [JsonProperty("type_name")]
        public string? TypeName { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal static GetOrganizationContactResponse FromDomain(OrganizationContact domain)
        {
            return new GetOrganizationContactResponse
            {
                Id = domain.Id,
                Value = domain.Value,
                AllowNotification = domain.AllowNotification,
                TypeId = domain.TypeId,
                TypeName = domain.TypeName,
                OrganizationId = domain.OrganizationId
            };
        }
    }

    public class CreateOrganizationContactRequest
    {
        [JsonProperty("value")]
        public string? Value { get; set; }
        
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }
        
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal OrganizationContact ToDomain()
        {
            return new OrganizationContact
            {
                Value = Value,
                AllowNotification = AllowNotification,
                TypeId = TypeId,
                OrganizationId = OrganizationId
            };
        }
    }

    public class UpdateOrganizationContactRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("value")]
        public string? Value { get; set; }
        
        [JsonProperty("allow_notification")]
        public bool? AllowNotification { get; set; }
        
        [JsonProperty("type_id")]
        public int TypeId { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal OrganizationContact ToDomain()
        {
            return new OrganizationContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                TypeId = TypeId,
                OrganizationId = OrganizationId
            };
        }
    }
}