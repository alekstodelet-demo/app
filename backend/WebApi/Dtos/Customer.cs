using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetCustomerResponse
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("pin")]
        public string Pin { get; set; }
            
        [JsonProperty("okpo")]
        public string Okpo { get; set; }
            
        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }
            
        [JsonProperty("ugns")]
        public string Ugns { get; set; }
            
        [JsonProperty("regNumber")]
        public string RegNumber { get; set; }
            
        [JsonProperty("organizationTypeId")]
        public int? OrganizationTypeId { get; set; }
            
        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }
            
        [JsonProperty("name")]
        public string Name { get; set; }
            
        [JsonProperty("address")]
        public string Address { get; set; }
            
        [JsonProperty("director")]
        public string Director { get; set; }
            
        [JsonProperty("nomer")]
        public string Nomer { get; set; }
            

        internal static GetCustomerResponse FromDomain(Customer domain)
        {
            return new GetCustomerResponse
            {
                Id = domain.Id,
                Pin = domain.Pin,
                Okpo = domain.Okpo,
                PostalCode = domain.PostalCode,
                Ugns = domain.Ugns,
                RegNumber = domain.RegNumber,
                OrganizationTypeId = domain.OrganizationTypeId,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                Name = domain.Name,
                Address = domain.Address,
                Director = domain.Director,
                Nomer = domain.Nomer,
                
            };
        }
    }

    public class CreateCustomerRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("ugns")]
        public string Ugns { get; set; }

        [JsonProperty("regNumber")]
        public string RegNumber { get; set; }

        [JsonProperty("organizationTypeId")]
        public int? OrganizationTypeId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("nomer")]
        public string Nomer { get; set; }


        internal Customer ToDomain()
        {
            return new Customer
            {
                Id = Id,
                Pin = Pin,
                Okpo = Okpo,
                PostalCode = PostalCode,
                Ugns = Ugns,
                RegNumber = RegNumber,
                OrganizationTypeId = OrganizationTypeId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                Name = Name,
                Address = Address,
                Director = Director,
                Nomer = Nomer,
                
            };
        }
    }

    public class UpdateCustomerRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("okpo")]
        public string Okpo { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("ugns")]
        public string Ugns { get; set; }

        [JsonProperty("regNumber")]
        public string RegNumber { get; set; }

        [JsonProperty("organizationTypeId")]
        public int? OrganizationTypeId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("createdBy")]
        public int? CreatedBy { get; set; }

        [JsonProperty("updatedBy")]
        public int? UpdatedBy { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("director")]
        public string Director { get; set; }

        [JsonProperty("nomer")]
        public string Nomer { get; set; }


        internal Customer ToDomain()
        {
            return new Customer
            {
                Id = Id,
                Pin = Pin,
                Okpo = Okpo,
                PostalCode = PostalCode,
                Ugns = Ugns,
                RegNumber = RegNumber,
                OrganizationTypeId = OrganizationTypeId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                Name = Name,
                Address = Address,
                Director = Director,
                Nomer = Nomer,
                
            };
        }
    }
}
