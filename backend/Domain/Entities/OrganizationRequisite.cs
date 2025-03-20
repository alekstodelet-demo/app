namespace Domain.Entities
{
    public class OrganizationRequisite : BaseLogDomain
    {
        public int Id { get; set; }
        public string? PaymentAccount { get; set; }
        public string? Bank { get; set; }
        public string? Bik { get; set; }
        public int OrganizationId { get; set; }
    }
}
