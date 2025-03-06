
namespace Domain.Entities
{
    public class ApplicationPaidInvoice : BaseLogDomain
    {
        public int id { get; set; }
        public DateTime? date { get; set; }
        public string payment_identifier { get; set; }
        public decimal sum { get; set; }
        public string bank_identifier { get; set; }
        public int application_id { get; set; }
        public string? tax { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public int? created_by { get; set; }
        public int? updated_by { get; set; }
    }
}
