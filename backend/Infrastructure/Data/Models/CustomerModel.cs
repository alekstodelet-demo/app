using Domain;

namespace Infrastructure.Data.Models
{
    public class CustomerModel : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
		public string Pin { get; set; }
		public string Okpo { get; set; }
		public string PostalCode { get; set; }
		public string Ugns { get; set; }
		public string RegNumber { get; set; }
		public int? OrganizationTypeId { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string Director { get; set; }
		public string Nomer { get; set; }
		
    }
}