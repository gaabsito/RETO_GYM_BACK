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
        public string? FotoPerfilURL { get; set; } // Nueva propiedad para URL de foto

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }

        [JsonIgnore]
        public string Password { get; set; } = "";
    }
}