using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string Pin { get; set; }
        
        [Required]
        public string TokenId { get; set; }
        
        [Required]
        public string Signature { get; set; }
        
        public string DeviceId { get; set; }
    }
    
    public class AuthResponse
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
    
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; }
    }
    
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
        public string DeviceId { get; set; }
    }
}
