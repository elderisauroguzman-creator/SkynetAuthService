using System;
using System.Collections.Generic;

namespace AuthService.Models
{
    public class RolUsuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
