using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Data.Models
{
    public class task_typeModel : BaseLogDomain
    {
        public int id { get; set; }
		public string name { get; set; }
		public string code { get; set; }
		public string description { get; set; }
		public bool? is_for_task { get; set; }
		public bool? is_for_subtask { get; set; }
		public string icon_color { get; set; }
		public int? svg_icon_id { get; set; }
		
    }
}