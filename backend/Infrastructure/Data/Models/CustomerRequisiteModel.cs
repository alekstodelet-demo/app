using Domain;

namespace Infrastructure.Data.Models
{
    public class CustomerRequisiteModel : BaseLogDomain
    {
        public int Id { get; set; }
		public string PaymentAccount { get; set; }
		public string Bank { get; set; }
		public string Bik { get; set; }
		public int OrganizationId { get; set; }
		
    }
}