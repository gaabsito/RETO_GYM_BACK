using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecetasAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; }

        [Required, MaxLength(50)]
        public string Apellido { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MinLength(6)]
        [JsonIgnore] // Oculta la contraseña en respuestas JSON
        public string Password { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public bool EstaActivo { get; set; } = true;
    }
}