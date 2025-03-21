using Domain;

namespace Infrastructure.Data.Models
{
    public class RepresentativeTypeModel : BaseLogDomain
    {
        public int Id { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		
    }
}