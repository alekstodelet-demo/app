using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class CustomerContact : BaseLogDomain
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public int CustomerId { get; set; }
        public bool? AllowNotification { get; set; }

    }
}
