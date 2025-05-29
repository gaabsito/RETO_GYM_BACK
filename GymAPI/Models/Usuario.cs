using System.Text.Json.Serialization;

namespace GymAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public int? Edad { get; set; }   // Nullable
        public float? Peso { get; set; } // Nullable
        public float? Altura { get; set; } // Nullable
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool EstaActivo { get; set; } = true;
        public string? FotoPerfilURL { get; set; }

        // NUEVO: Campo para identificar administradores
        public bool EsAdmin { get; set; } = false;

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }

        [JsonIgnore]
        public string Password { get; set; } = "";
    }
}