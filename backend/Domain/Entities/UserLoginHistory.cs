namespace Domain.Entities
{
    public class UserLoginHistory
    {
        public int id { get; set; }
        public string? user_id { get; set; }
        public string? ip_address { get; set; }
        public string? device { get; set; }
        public string? browser { get; set; }
        public string? os { get; set; }
        public string? start_time { get; set; }
        public string? end_time { get; set; }
    }
}
