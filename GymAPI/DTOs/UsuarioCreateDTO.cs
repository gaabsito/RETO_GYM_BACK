using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class UsuarioCreateDTO
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";
    }
}
