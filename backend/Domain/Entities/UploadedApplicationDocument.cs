﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UploadedApplicationDocument
    {
        public int id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? created_by { get; set; }
        public DateTime? updated_by { get; set; }
        public int application_document { get; set; }
        public string name { get; set; }

        public int service_document_id { get; set; }
        public int file_id { get; set; }
    }
}
