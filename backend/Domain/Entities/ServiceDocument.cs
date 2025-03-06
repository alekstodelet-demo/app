using System.Xml.Linq;

namespace Domain.Entities
{
    public class ServiceDocument : BaseLogDomain
    {
        public int id { get; set; }
        public int? service_id { get; set; }
        public int? application_document_id { get; set; }
        public string? application_document_name { get; set; }
        public bool? is_required { get; set; }
    }
}
