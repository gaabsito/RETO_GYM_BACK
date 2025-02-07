using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EntrenamientoCreateDTO
    {
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = "";

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = "";

        [Range(5, 300, ErrorMessage = "La duración debe estar entre 5 y 300 minutos.")]
        public int DuracionMinutos { get; set; }

        [Required]
        [RegularExpression("Fácil|Media|Difícil", ErrorMessage = "Dificultad debe ser 'Fácil', 'Media' o 'Difícil'.")]
        public string Dificultad { get; set; } = "";

        public bool Publico { get; set; } = true;

        public int? AutorID { get; set; }
    }
}
