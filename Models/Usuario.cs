using System;

namespace AuthService.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdRolUsuario { get; set; } // FK
        public RolUsuario Rol { get; set; }   // navegación
        public string UserName { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
