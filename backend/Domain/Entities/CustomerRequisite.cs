
namespace Domain.Entities
{
    public class CustomerRequisite : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
		public string PaymentAccount { get; set; }
		public string Bank { get; set; }
		public string Bik { get; set; }
		public int OrganizationId { get; set; }
		
    }
}