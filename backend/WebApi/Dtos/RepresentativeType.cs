using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetRepresentativeTypeResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("code")]
        public string? Code { get; set; }

        internal static GetRepresentativeTypeResponse FromDomain(RepresentativeType domain)
        {
            return new GetRepresentativeTypeResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description,
                Code = domain.Code
            };
        }
    }

    public class CreateRepresentativeTypeRequest
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("code")]
        public string? Code { get; set; }

        internal RepresentativeType ToDomain()
        {
            return new RepresentativeType
            {
                Name = Name,
                Description = Description,
                Code = Code
            };
        }
    }

    public class UpdateRepresentativeTypeRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string? Name { get; set; }
        
        [JsonProperty("description")]
        public string? Description { get; set; }
        
        [JsonProperty("code")]
        public string? Code { get; set; }

        internal RepresentativeType ToDomain()
        {
            return new RepresentativeType
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Code = Code
            };
        }
    }
}