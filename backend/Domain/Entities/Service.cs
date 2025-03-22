using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class Service : BaseLogDomain, IBaseDomain
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int? DayCount { get; set; }
        public int? WorkflowId { get; set; }
        public decimal? Price { get; set; }
        public string? WorkflowName { get; set; }
        public bool? IsActive { get; set; }
    }
}
