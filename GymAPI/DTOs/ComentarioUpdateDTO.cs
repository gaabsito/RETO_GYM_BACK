using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class ComentarioUpdateDTO
    {
        [StringLength(500, ErrorMessage = "El contenido no puede superar los 500 caracteres.")]
        public string? Contenido { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int? Calificacion { get; set; }
    }
}