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
            
        [JsonProperty("allowNotification")]
        public bool? AllowNotification { get; set; }
            
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("rTypeId")]
        public int? RTypeId { get; set; }
            
        [JsonProperty("organizationId")]
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
            
        [JsonProperty("allowNotification")]
        public bool? AllowNotification { get; set; }
            
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("rTypeId")]
        public int? RTypeId { get; set; }
            
        [JsonProperty("organizationId")]
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

        [JsonProperty("allowNotification")]
        public bool? AllowNotification { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("rTypeId")]
        public int? RTypeId { get; set; }

        [JsonProperty("organizationId")]
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
