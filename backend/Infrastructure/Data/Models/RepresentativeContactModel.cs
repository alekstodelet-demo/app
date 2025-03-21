using Domain;

namespace Infrastructure.Data.Models
{
    public class RepresentativeContactModel : BaseLogDomain
    {
        public int Id { get; set; }
		public string Value { get; set; }
		public bool? AllowNotification { get; set; }
		public int? RTypeId { get; set; }
		public int RepresentativeId { get; set; }
		
    }
}