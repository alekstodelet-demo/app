using Domain;

namespace Infrastructure.Data.Models
{
    public class CustomerContactModel : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
		public string Value { get; set; }
		public bool? AllowNotification { get; set; }
		public int? RTypeId { get; set; }
		public int OrganizationId { get; set; }
		
    }
}