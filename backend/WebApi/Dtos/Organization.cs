using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetOrganizationResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("director")]
        public string? Director { get; set; }
        [JsonProperty("nomer")]
        public string? Nomer { get; set; }
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        [JsonProperty("okpo")]
        public string? Okpo { get; set; }
        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }
        [JsonProperty("organization_type_name")]
        public string? OrganizationTypeName { get; set; }
        [JsonProperty("postal_code")]
        public string? PostalCode { get; set; }
        [JsonProperty("ugns")]
        public string? Ugns { get; set; }
        [JsonProperty("reg_number")]
        public string? RegNumber { get; set; }

        internal static GetOrganizationResponse FromDomain(Organization domain)
        {
            return new GetOrganizationResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Address = domain.Address,
                Director = domain.Director,
                Nomer = domain.Nomer,
                Pin = domain.Pin,
                Okpo = domain.Okpo,
                OrganizationTypeId = domain.OrganizationTypeId,
                OrganizationTypeName = domain.OrganizationTypeName,
                PostalCode = domain.PostalCode,
                Ugns = domain.Ugns,
                RegNumber = domain.RegNumber
            };
        }
    }

    public class CreateOrganizationRequest
    {
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("director")]
        public string? Director { get; set; }
        [JsonProperty("nomer")]
        public string? Nomer { get; set; }
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        [JsonProperty("okpo")]
        public string? Okpo { get; set; }
        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }
        [JsonProperty("postal_code")]
        public string? PostalCode { get; set; }
        [JsonProperty("ugns")]
        public string? Ugns { get; set; }
        [JsonProperty("reg_number")]
        public string? RegNumber { get; set; }

        internal Organization ToDomain()
        {
            return new Organization
            {
                Name = Name,
                Address = Address,
                Director = Director,
                Nomer = Nomer,
                Pin = Pin,
                Okpo = Okpo,
                OrganizationTypeId = OrganizationTypeId,
                PostalCode = PostalCode,
                Ugns = Ugns,
                RegNumber = RegNumber
            };
        }
    }

    public class UpdateOrganizationRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("director")]
        public string? Director { get; set; }
        [JsonProperty("nomer")]
        public string? Nomer { get; set; }
        [JsonProperty("pin")]
        public string? Pin { get; set; }
        [JsonProperty("okpo")]
        public string? Okpo { get; set; }
        [JsonProperty("organization_type_id")]
        public int? OrganizationTypeId { get; set; }
        [JsonProperty("postal_code")]
        public string? PostalCode { get; set; }
        [JsonProperty("ugns")]
        public string? Ugns { get; set; }
        [JsonProperty("reg_number")]
        public string? RegNumber { get; set; }

        internal Organization ToDomain()
        {
            return new Organization
            {
                Id = Id,
                Name = Name,
                Address = Address,
                Director = Director,
                Nomer = Nomer,
                Pin = Pin,
                Okpo = Okpo,
                OrganizationTypeId = OrganizationTypeId,
                PostalCode = PostalCode,
                Ugns = Ugns,
                RegNumber = RegNumber
            };
        }
    }
}