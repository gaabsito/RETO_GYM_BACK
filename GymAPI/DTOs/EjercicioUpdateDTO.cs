using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class EjercicioUpdateDTO
    {
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Nombre { get; set; }

        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string? Descripcion { get; set; }

        [StringLength(50, ErrorMessage = "El grupo muscular no puede superar los 50 caracteres.")]
        public string? GrupoMuscular { get; set; }

        public string? ImagenURL { get; set; }  // Se elimina la validación de URL para permitir valores vacíos o nulos.

        public bool? EquipamientoNecesario { get; set; }
    }
}
