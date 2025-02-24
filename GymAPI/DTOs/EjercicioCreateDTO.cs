using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EjercicioCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = "";

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Descripcion { get; set; }

        [StringLength(50, ErrorMessage = "El grupo muscular no puede superar los 50 caracteres.")]
        public string? GrupoMuscular { get; set; }

        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string? ImagenURL { get; set; }
        
        [Url(ErrorMessage = "Debe ser una URL válida.")]
        public string? VideoURL { get; set; }

        public bool EquipamientoNecesario { get; set; } = false;
    }
}
