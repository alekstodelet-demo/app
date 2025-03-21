using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetOrganizationTypeResponse
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("description_kg")]
        public string DescriptionKg { get; set; }
            
        [JsonProperty("text_color")]
        public string TextColor { get; set; }
            
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("name_kg")]
        public string NameKg { get; set; }
            

        internal static GetOrganizationTypeResponse FromDomain(OrganizationType domain)
        {
            return new GetOrganizationTypeResponse
            {
                Id = domain.Id,
                DescriptionKg = domain.DescriptionKg,
                TextColor = domain.TextColor,
                BackgroundColor = domain.BackgroundColor,
                Name = domain.Name,
                Description = domain.Description,
                Code = domain.Code,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                NameKg = domain.NameKg,
                
            };
        }
    }

    public class CreateOrganizationTypeRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("description_kg")]
        public string DescriptionKg { get; set; }
            
        [JsonProperty("text_color")]
        public string TextColor { get; set; }
            
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("name_kg")]
        public string NameKg { get; set; }
            

        internal OrganizationType ToDomain()
        {
            return new OrganizationType
            {
                Id = Id,
                DescriptionKg = DescriptionKg,
                TextColor = TextColor,
                BackgroundColor = BackgroundColor,
                Name = Name,
                Description = Description,
                Code = Code,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                NameKg = NameKg,
                
            };
        }
    }

    public class UpdateOrganizationTypeRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("description_kg")]
        public string DescriptionKg { get; set; }
            
        [JsonProperty("text_color")]
        public string TextColor { get; set; }
            
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("description")]
        public string Description { get; set; }
            
        [JsonProperty("code")]
        public string Code { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("name_kg")]
        public string NameKg { get; set; }
            

        internal OrganizationType ToDomain()
        {
            return new OrganizationType
            {
                Id = Id,
                DescriptionKg = DescriptionKg,
                TextColor = TextColor,
                BackgroundColor = BackgroundColor,
                Name = Name,
                Description = Description,
                Code = Code,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                NameKg = NameKg,
                
            };
        }
    }
}
