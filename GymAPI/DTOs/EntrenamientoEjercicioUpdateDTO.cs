using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EntrenamientoEjercicioUpdateDTO
    {
        [Range(1, 10)]
        public int? Series { get; set; }

        [Range(1, 50)]
        public int? Repeticiones { get; set; }

        [Range(10, 300)]
        public int? DescansoSegundos { get; set; }

        [StringLength(100)]
        public string? Notas { get; set; }
    }
}
