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
            
        [JsonProperty("representativeId")]
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

        [JsonProperty("representativeId")]
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

        [JsonProperty("representativeId")]
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
