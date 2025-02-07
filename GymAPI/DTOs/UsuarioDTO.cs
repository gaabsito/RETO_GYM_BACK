using System;

namespace GymAPI.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public bool EstaActivo { get; set; }
    }
}
