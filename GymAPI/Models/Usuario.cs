using System.Text.Json.Serialization;

namespace GymAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public int? Edad { get; set; }
        public float? Peso { get; set; }
        public float? Altura { get; set; }
        
        // Nuevos campos para objetivos
        public float? ObjetivoPeso { get; set; }
        public float? ObjetivoIMC { get; set; }
        public float? ObjetivoPorcentajeGrasa { get; set; }
        
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool EstaActivo { get; set; } = true;

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordExpires { get; set; }

        [JsonIgnore]
        public string Password { get; set; } = "";
    }
}