namespace WebApi.Dtos
{
    public class AuthRequest
    {
        public string Pin { get; set; }
        public string TokenId { get; set; }
        public string Signature { get; set; }
    }
}
