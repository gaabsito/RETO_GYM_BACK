namespace GymAPI.Models
{
    public class Entrenamiento
    {
        public int EntrenamientoID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int DuracionMinutos { get; set; }
        public string Dificultad { get; set; } = string.Empty;
        public string? ImagenURL { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Publico { get; set; }
        public int? AutorID { get; set; }

        // Relación con los ejercicios
        public ICollection<EntrenamientoEjercicio>? EntrenamientoEjercicios { get; set; } = new List<EntrenamientoEjercicio>();
    }
}
