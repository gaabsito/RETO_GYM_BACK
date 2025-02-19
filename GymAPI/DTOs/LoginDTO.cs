using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = "";
    }
}