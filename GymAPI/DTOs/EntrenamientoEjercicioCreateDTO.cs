using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EntrenamientoEjercicioCreateDTO
    {
        [Required]
        public int EntrenamientoID { get; set; }

        [Required]
        public int EjercicioID { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Las series deben estar entre 1 y 10.")]
        public int Series { get; set; }

        [Required]
        [Range(1, 50, ErrorMessage = "Las repeticiones deben estar entre 1 y 50.")]
        public int Repeticiones { get; set; }

        [Required]
        [Range(10, 300, ErrorMessage = "El descanso debe estar entre 10 y 300 segundos.")]
        public int DescansoSegundos { get; set; }

        [StringLength(100)]
        public string? Notas { get; set; }
    }
}
