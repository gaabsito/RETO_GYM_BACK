using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GymAPI.DTOs
{
    public class AdminUsuarioUpdateDTO
    {
        [StringLength(50)]
        [JsonPropertyName("nombre")]
        public string? Nombre { get; set; }

        [StringLength(50)]
        [JsonPropertyName("apellido")]
        public string? Apellido { get; set; }

        [EmailAddress]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [MinLength(6)]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("edad")]
        public int? Edad { get; set; }
        
        [JsonPropertyName("altura")]
        public float? Altura { get; set; }
        
        [JsonPropertyName("peso")]
        public float? Peso { get; set; }
        
        // CRÍTICO: Mapear correctamente el campo esAdmin del frontend
        [JsonPropertyName("esAdmin")]
        public bool? EsAdmin { get; set; }

        // CRÍTICO: Mapear correctamente el campo estaActivo del frontend
        [JsonPropertyName("estaActivo")]
        public bool? EstaActivo { get; set; }
    }
}