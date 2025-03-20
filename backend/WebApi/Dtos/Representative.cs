using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetRepresentativeResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("last_name")]
        public string? LastName { get; set; }
        
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }
        
        [JsonProperty("second_name")]
        public string? SecondName { get; set; }
        
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }
        
        [JsonProperty("type_code")]
        public int? TypeCode { get; set; }
        
        [JsonProperty("representative_type_id")]
        public int? RepresentativeTypeId { get; set; }

        internal static GetRepresentativeResponse FromDomain(Representative domain)
        {
            return new GetRepresentativeResponse
            {
                Id = domain.Id,
                LastName = domain.LastName,
                FirstName = domain.FirstName,
                SecondName = domain.SecondName,
                Pin = domain.Pin,
                CompanyId = domain.CompanyId,
                TypeCode = domain.TypeCode,
                RepresentativeTypeId = domain.RepresentativeTypeId
            };
        }
    }

    public class CreateRepresentativeRequest
    {
        [JsonProperty("last_name")]
        public string? LastName { get; set; }
        
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }
        
        [JsonProperty("second_name")]
        public string? SecondName { get; set; }
        
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }
        
        [JsonProperty("type_code")]
        public int? TypeCode { get; set; }
        
        [JsonProperty("representative_type_id")]
        public int? RepresentativeTypeId { get; set; }

        internal Representative ToDomain()
        {
            return new Representative
            {
                LastName = LastName,
                FirstName = FirstName,
                SecondName = SecondName,
                Pin = Pin,
                CompanyId = CompanyId,
                TypeCode = TypeCode,
                RepresentativeTypeId = RepresentativeTypeId
            };
        }
    }

    public class UpdateRepresentativeRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("last_name")]
        public string? LastName { get; set; }
        
        [JsonProperty("first_name")]
        public string? FirstName { get; set; }
        
        [JsonProperty("second_name")]
        public string? SecondName { get; set; }
        
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        
        [JsonProperty("company_id")]
        public int? CompanyId { get; set; }
        
        [JsonProperty("type_code")]
        public int? TypeCode { get; set; }
        
        [JsonProperty("representative_type_id")]
        public int? RepresentativeTypeId { get; set; }

        internal Representative ToDomain()
        {
            return new Representative
            {
                Id = Id,
                LastName = LastName,
                FirstName = FirstName,
                SecondName = SecondName,
                Pin = Pin,
                CompanyId = CompanyId,
                TypeCode = TypeCode,
                RepresentativeTypeId = RepresentativeTypeId
            };
        }
    }
}