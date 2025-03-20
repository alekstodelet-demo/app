using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetContactTypeResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("additional")]
        public string Additional { get; set; }
        [JsonProperty("regex")]
        public string Regex { get; set; }


        internal static GetContactTypeResponse FromDomain(ContactType domain)
        {
            return new GetContactTypeResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Description = domain.Description,
                Code = domain.Code,
                Additional = domain.Additional,
                Regex = domain.Regex,
            };
        }
    }

    public class CreateContactTypeRequest
    {

        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("additional")]
        public string Additional { get; set; }
        [JsonProperty("regex")]
        public string Regex { get; set; }

        internal ContactType ToDomain()
        {
            return new ContactType
            {
                Name = Name,
                Description = Description,
                Code = Code,
                Additional = Additional,
                Regex = Regex,
            };
        }
    }

    public class UpdateContactTypeRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("additional")]
        public string Additional { get; set; }
        [JsonProperty("regex")]
        public string Regex { get; set; }

        internal ContactType ToDomain()
        {
            return new ContactType
            {
                Name = Name,
                Description = Description,
                Code = Code,
                Additional = Additional,
                Regex = Regex,
            };
        }
    }
}
