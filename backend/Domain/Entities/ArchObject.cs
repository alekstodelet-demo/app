
namespace Domain.Entities
{
    public class ArchObject : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Identifier { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string? Description { get; set; }
        public double? XCoordinate { get; set; }
        public double? YCoordinate { get; set; }
    }
}
