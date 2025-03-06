using System.Xml.Linq;

namespace Domain.Entities
{
    public class Service : BaseLogDomain
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? short_name { get; set; }
        public string? code { get; set; }
        public string? description { get; set; }
        public int? day_count { get; set; }
        public int? workflow_id { get; set; }
        public decimal? price { get; set; }
        public string? workflow_name { get; set; }
        public bool? is_active { get; set; }
    }
    public class ResultDashboard
    {
        public List<long> counts { get; set; }
        public List<string> names { get; set; }
    }
    public class ChartTableDataDashboard
    {
        public string register { get; set; }
        public int employee_id { get; set; }
        public int count { get; set; }
    }
    public class AppCountDashboradData
    {
        public int all_count { get; set; }
        public int tech_accepted_count { get; set; }
        public int tech_declined_count { get; set; }
        public int done_count { get; set; }
        public int at_work_count { get; set; }
    }
}
