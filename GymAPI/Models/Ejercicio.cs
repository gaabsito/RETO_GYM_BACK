namespace GymAPI.Models
{
    public class Ejercicio
    {
        public int EjercicioID { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? GrupoMuscular { get; set; }
        public string? ImagenURL { get; set; }
        public string? VideoURL { get; set; }
        public bool EquipamientoNecesario { get; set; }
    }
}
