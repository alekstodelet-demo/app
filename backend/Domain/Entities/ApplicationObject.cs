using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationObject : BaseLogDomain
    {
        public int Id { get; set; }
		public int ApplicationId { get; set; }
		public int ArchObjectId { get; set; }
		
    }
}