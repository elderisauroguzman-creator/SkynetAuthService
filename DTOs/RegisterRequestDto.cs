namespace AuthService.DTOs
{
    public class RegisterRequestDto
    {
        public string Nombre { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdRolUsuario { get; set; }
    }
}
