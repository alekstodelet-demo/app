namespace Domain.Entities
{
    public class OrganizationContact : BaseLogDomain
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public bool? AllowNotification { get; set; }
        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public int OrganizationId { get; set; }
    }
}