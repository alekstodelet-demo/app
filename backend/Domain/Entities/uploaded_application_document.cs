using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class uploaded_application_document : BaseLogDomain
    {
        public int id { get; set; }
		public int? file_id { get; set; }
		public int? application_document_id { get; set; }
		public string name { get; set; }
		public int? service_document_id { get; set; }
        public string app_doc_name { get; set; }
		public DateTime? created_at { get; set; }
		public DateTime? updated_at { get; set; }
		public int? created_by { get; set; }
		public int? updated_by { get; set; }
        public File document { get; set; }
        public bool? is_outcome { get; set; }
        public string? document_number { get; set; }
        public List<string>? app_docs { get; set; }

    } 

    public class CustomUploadedDocument
    {
        public int id { get; set; }
        public string doc_name { get; set; }
        public int app_doc_id { get; set; }
        public bool? is_required { get; set; }
        public string type_name { get; set; }
        public int? upload_id { get; set; }
        public string upload_name { get; set; }
        public DateTime? created_at { get; set; }
        public int? file_id { get; set; }
        public string file_name { get; set; }
        public int? created_by { get; set; }
        public bool? is_outcome { get; set; }
        public string? document_number { get; set; }

    }

    public class CustomAttachedDocument
    {
        public int id { get; set; }
        public int file_id { get; set; }
        public int application_id { get; set; }
        public string number { get; set; }
        public string service_name { get; set; }
        public DateTime? created_at { get; set; }
        public string file_name { get; set; }
        public int service_document_id { get; set; }
        public bool? is_outcome { get; set; }
        public string? document_number { get; set; }

    }
}