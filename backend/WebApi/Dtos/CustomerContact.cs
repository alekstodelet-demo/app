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
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            

        internal static GetCustomerContactResponse FromDomain(CustomerContact domain)
        {
            return new GetCustomerContactResponse
            {
                Id = domain.Id,
                Value = domain.Value,
                AllowNotification = domain.AllowNotification,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                RTypeId = domain.RTypeId,
                OrganizationId = domain.OrganizationId,
                
            };
        }
    }

    public class CreateCustomerContactRequest
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
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            

        internal CustomerContact ToDomain()
        {
            return new CustomerContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                RTypeId = RTypeId,
                OrganizationId = OrganizationId,
                
            };
        }
    }

    public class UpdateCustomerContactRequest
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
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            

        internal CustomerContact ToDomain()
        {
            return new CustomerContact
            {
                Id = Id,
                Value = Value,
                AllowNotification = AllowNotification,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                RTypeId = RTypeId,
                OrganizationId = OrganizationId,
                
            };
        }
    }
}
