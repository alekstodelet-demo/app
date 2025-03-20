using Domain.Entities;
using Newtonsoft.Json;

namespace WebApi.Dtos
{
    public class GetOrganizationRequisiteResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("payment_account")]
        public string? PaymentAccount { get; set; }
        
        [JsonProperty("bank")]
        public string? Bank { get; set; }
        
        [JsonProperty("bik")]
        public string? Bik { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal static GetOrganizationRequisiteResponse FromDomain(OrganizationRequisite domain)
        {
            return new GetOrganizationRequisiteResponse
            {
                Id = domain.Id,
                PaymentAccount = domain.PaymentAccount,
                Bank = domain.Bank,
                Bik = domain.Bik,
                OrganizationId = domain.OrganizationId
            };
        }
    }

    public class CreateOrganizationRequisiteRequest
    {
        [JsonProperty("payment_account")]
        public string? PaymentAccount { get; set; }
        
        [JsonProperty("bank")]
        public string? Bank { get; set; }
        
        [JsonProperty("bik")]
        public string? Bik { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal OrganizationRequisite ToDomain()
        {
            return new OrganizationRequisite
            {
                PaymentAccount = PaymentAccount,
                Bank = Bank,
                Bik = Bik,
                OrganizationId = OrganizationId
            };
        }
    }

    public class UpdateOrganizationRequisiteRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("payment_account")]
        public string? PaymentAccount { get; set; }
        
        [JsonProperty("bank")]
        public string? Bank { get; set; }
        
        [JsonProperty("bik")]
        public string? Bik { get; set; }
        
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }

        internal OrganizationRequisite ToDomain()
        {
            return new OrganizationRequisite
            {
                Id = Id,
                PaymentAccount = PaymentAccount,
                Bank = Bank,
                Bik = Bik,
                OrganizationId = OrganizationId
            };
        }
    }
}