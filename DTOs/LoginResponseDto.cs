namespace AuthService.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Rol { get; set; }
        public DateTime Expiration { get; set; }
    }
}
