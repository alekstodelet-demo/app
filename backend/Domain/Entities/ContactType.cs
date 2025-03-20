using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Domain.Entities
{
    public class ContactType : BaseLogDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Additional { get; set; }
        public string Regex { get; set; }

    }
}
