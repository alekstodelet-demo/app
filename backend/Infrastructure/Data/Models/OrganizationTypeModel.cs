using Domain;

namespace Infrastructure.Data.Models
{
    public class OrganizationTypeModel : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
		public string DescriptionKg { get; set; }
		public string TextColor { get; set; }
		public string BackgroundColor { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Code { get; set; }
		public string NameKg { get; set; }
		
    }
}