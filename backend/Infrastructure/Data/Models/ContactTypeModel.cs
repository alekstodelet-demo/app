using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Models
{
    public class ContactTypeModel : BaseLogDomain
    {
        public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		
    }
}