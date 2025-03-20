using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    [JsonObject(NamingStrategyType = null)]
    public class GetOrganizationTypeResponse
    {
        [JsonProperty("id")] 
        public int Id { get; set; }
        [JsonProperty("name")] 
        public string? Name { get; set; }
        [JsonProperty("description")] 
        public string? Description { get; set; }
        [JsonProperty("code")] 
        public string? Code { get; set; }
        [JsonProperty("name_kg")] 
        public string? NameKg { get; set; }
        [JsonProperty("description_kg")] 
        public string? DescriptionKg { get; set; }
        [JsonProperty("text_color")] 
        public string? TextColor { get; set; }
        [JsonProperty("background_color")] 
        public string? BackgroundColor { get; set; }

        internal static GetOrganizationTypeResponse FromDomain(OrganizationType domain)
        {
            return new GetOrganizationTypeResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description,
                Code = domain.Code,
                NameKg = domain.NameKg,
                DescriptionKg = domain.DescriptionKg,
                TextColor = domain.TextColor,
                BackgroundColor = domain.BackgroundColor
            };
        }
    }

    public class CreateOrganizationTypeRequest
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("code")]
        public string? Code { get; set; }
        
        [JsonProperty("name_kg")]
        public string? NameKg { get; set; }
        
        [JsonProperty("description_kg")]
        public string? DescriptionKg { get; set; }
        
        [JsonProperty("text_color")]
        public string? TextColor { get; set; }
        
        [JsonProperty("background_color")]
        public string? BackgroundColor { get; set; }

        internal OrganizationType ToDomain()
        {
            return new OrganizationType
            {
                Name = Name,
                Description = Description,
                Code = Code,
                NameKg = NameKg,
                DescriptionKg = DescriptionKg,
                TextColor = TextColor,
                BackgroundColor = BackgroundColor
            };
        }
    }

    public class UpdateOrganizationTypeRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("code")]
        public string? Code { get; set; }
        
        [JsonProperty("name_kg")]
        public string? NameKg { get; set; }
        
        [JsonProperty("description_kg")]
        public string? DescriptionKg { get; set; }
        
        [JsonProperty("text_color")]
        public string? TextColor { get; set; }
        
        [JsonProperty("background_color")]
        public string? BackgroundColor { get; set; }

        internal OrganizationType ToDomain()
        {
            return new OrganizationType
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Code = Code,
                NameKg = NameKg,
                DescriptionKg = DescriptionKg,
                TextColor = TextColor,
                BackgroundColor = BackgroundColor
            };
        }
    }
}