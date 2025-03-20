using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Models
{
    public class CustomerModel : BaseLogDomain
    {
        public int Id { get; set; }
		public string? Pin { get; set; }
		
    }
}