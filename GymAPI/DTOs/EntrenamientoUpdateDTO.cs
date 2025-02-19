using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EntrenamientoUpdateDTO
    {
        [StringLength(100)]
        public string? Titulo { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        [Range(5, 300, ErrorMessage = "La duración debe estar entre 5 y 300 minutos.")]
        public int? DuracionMinutos { get; set; }

        [RegularExpression("Fácil|Media|Difícil", ErrorMessage = "Dificultad debe ser 'Fácil', 'Media' o 'Difícil'.")]
        public string? Dificultad { get; set; }

        [Url(ErrorMessage = "Debe proporcionar una URL válida para la imagen.")]
        [StringLength(255)]
        public string? ImagenURL { get; set; }

        public bool? Publico { get; set; }
    }
}