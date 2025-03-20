using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Models
{
    public class CustomerContactModel : BaseLogDomain
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public bool? AllowNotification { get; set; }

    }
}