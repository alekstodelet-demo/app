using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Infrastructure.Data.Models
{
    public class uploaded_application_documentModel : BaseLogDomain
    {
        public int id { get; set; }
		public int? file_id { get; set; }
		public int? application_document_id { get; set; }
		public string name { get; set; }
		public int? service_document_id { get; set; }
		public string? document_number { get; set; }


    }
}