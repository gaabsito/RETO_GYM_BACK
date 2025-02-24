using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class UsuarioUpdateDTO
    {
        [StringLength(50)]
        public string? Nombre { get; set; }

        [StringLength(50)]
        public string? Apellido { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MinLength(6)]
        public string? Password { get; set; }

        public int? Edad { get; set; }
        public float? Altura { get; set; }
        public float? Peso { get; set; }
    }
}
