namespace Domain.Entities
{
    public class File : BaseLogDomain
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? path { get; set; }
        public byte[] body { get; set; }
    }
}
