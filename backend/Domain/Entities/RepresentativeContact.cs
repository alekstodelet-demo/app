namespace Domain.Entities
{
    public class RepresentativeContact : BaseLogDomain
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public bool? AllowNotification { get; set; }
        public int TypeId { get; set; }
        public int RepresentativeId { get; set; }
    }
}
