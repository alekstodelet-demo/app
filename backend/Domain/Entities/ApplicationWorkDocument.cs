
namespace Domain.Entities
{
    public class ApplicationWorkDocument : BaseLogDomain
    {
        public int id { get; set; }
        public int? file_id { get; set; }
        public int? task_id { get; set; }
        public string comment { get; set; }
        public int? structure_employee_id { get; set; }
        public File document { get; set; }
        public string? file_name { get; set; }
        public string? employee_name { get; set; }
        public string? task_name { get; set; }
        public int? structure_id { get; set; }
        public int? id_type { get; set; }
        public string id_type_name { get; set; }
        public string id_type_code { get; set; }
        public string? metadata { get; set; }
        public string? document_name { get; set; }
        public string? document_body { get; set; }
        public DateTime? deactivated_at { get; set; }
        public int? deactivated_by { get; set; }
        public bool? is_active { get; set; }
        public string? reason_deactivated { get; set; }
    }
}
