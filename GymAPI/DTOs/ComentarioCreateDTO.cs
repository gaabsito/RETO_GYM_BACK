using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class ComentarioCreateDTO
    {
        [Required(ErrorMessage = "El EntrenamientoID es requerido.")]
        public int EntrenamientoID { get; set; }

        [Required(ErrorMessage = "El UsuarioID es requerido.")]
        public int UsuarioID { get; set; }

        [Required(ErrorMessage = "El contenido es requerido.")]
        [StringLength(500, ErrorMessage = "El contenido no puede superar los 500 caracteres.")]
        public string Contenido { get; set; } = "";

        [Required(ErrorMessage = "La calificación es requerida.")]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5.")]
        public int Calificacion { get; set; }
    }
}