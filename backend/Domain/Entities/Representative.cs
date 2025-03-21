
namespace Domain.Entities
{
    public class Representative : BaseLogDomain
    {
        public int Id { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public string Pin { get; set; }
		public int CompanyId { get; set; }
		public bool? HasAccess { get; set; }
		public int TypeId { get; set; }
		public string LastName { get; set; }
		
    }
}