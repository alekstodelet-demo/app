using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetRepresentativeTypeResponse
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            

        internal static GetRepresentativeTypeResponse FromDomain(RepresentativeType domain)
        {
            return new GetRepresentativeTypeResponse
            {
                Id = domain.Id,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                Description = domain.Description,
                Name = domain.Name,
                Code = domain.Code,
                
            };
        }
    }

    public class CreateRepresentativeTypeRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            

        internal RepresentativeType ToDomain()
        {
            return new RepresentativeType
            {
                Id = Id,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                Description = Description,
                Name = Name,
                Code = Code,
                
            };
        }
    }

    public class UpdateRepresentativeTypeRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            

        internal RepresentativeType ToDomain()
        {
            return new RepresentativeType
            {
                Id = Id,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                Description = Description,
                Name = Name,
                Code = Code,
                
            };
        }
    }
}
