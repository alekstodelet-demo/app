﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArchiveObject : BaseLogDomain
    {
        public int id { get; set; }
        public string? doc_number { get; set; }
        public string? address { get; set; }
        public string? customer { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string? layer { get; set; }
        public string? description { get; set; }
        public DateTime? date_setplan { get; set; }
        public int? quantity_folder { get; set; }
        public int? status_dutyplan_object_id { get; set; }
        public string status_dutyplan_object_code { get; set; }
        public int? archirecture_process_id { get; set; }
        public int? archirecture_process_status_id { get; set; }
        public string archirecture_process_status_code { get; set; }
        public string archirecture_process_status_name { get; set; }
        public string? archive_folders { get; set; }
        public List<customers_for_archive_object> customers_for_archive_object { get; set; }
    }
    
    public class ArchiveObjectFilter
    {
        public string? search { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string? sort_by { get; set; }
        public string? sort_type { get; set; }
        public int status_id { get; set; }
    }
}
