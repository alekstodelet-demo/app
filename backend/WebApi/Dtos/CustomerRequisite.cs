using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebApi.Dtos
{
    public class GetCustomerRequisiteResponse
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }
            
        [JsonProperty("bank")]
        public string Bank { get; set; }
            
        [JsonProperty("bik")]
        public string Bik { get; set; }
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            

        internal static GetCustomerRequisiteResponse FromDomain(CustomerRequisite domain)
        {
            return new GetCustomerRequisiteResponse
            {
                Id = domain.Id,
                PaymentAccount = domain.PaymentAccount,
                Bank = domain.Bank,
                Bik = domain.Bik,
                OrganizationId = domain.OrganizationId,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt,
                CreatedBy = domain.CreatedBy,
                UpdatedBy = domain.UpdatedBy,
                
            };
        }
    }

    public class CreateCustomerRequisiteRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }
            
        [JsonProperty("bank")]
        public string Bank { get; set; }
            
        [JsonProperty("bik")]
        public string Bik { get; set; }
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            

        internal CustomerRequisite ToDomain()
        {
            return new CustomerRequisite
            {
                Id = Id,
                PaymentAccount = PaymentAccount,
                Bank = Bank,
                Bik = Bik,
                OrganizationId = OrganizationId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                
            };
        }
    }

    public class UpdateCustomerRequisiteRequest
    {
        
        [JsonProperty("id")]
        public int Id { get; set; }
            
        [JsonProperty("payment_account")]
        public string PaymentAccount { get; set; }
            
        [JsonProperty("bank")]
        public string Bank { get; set; }
            
        [JsonProperty("bik")]
        public string Bik { get; set; }
            
        [JsonProperty("organization_id")]
        public int OrganizationId { get; set; }
            
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
            
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
            
        [JsonProperty("created_by")]
        public int? CreatedBy { get; set; }
            
        [JsonProperty("updated_by")]
        public int? UpdatedBy { get; set; }
            

        internal CustomerRequisite ToDomain()
        {
            return new CustomerRequisite
            {
                Id = Id,
                PaymentAccount = PaymentAccount,
                Bank = Bank,
                Bik = Bik,
                OrganizationId = OrganizationId,
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                CreatedBy = CreatedBy,
                UpdatedBy = UpdatedBy,
                
            };
        }
    }
}
