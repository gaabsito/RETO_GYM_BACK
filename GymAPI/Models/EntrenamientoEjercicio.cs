namespace GymAPI.Models
{
    public class EntrenamientoEjercicio
    {
        public int EntrenamientoID { get; set; }
        public int EjercicioID { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public int DescansoSegundos { get; set; }
        public string? Notas { get; set; }

        public Entrenamiento? Entrenamiento { get; set; }
        public Ejercicio? Ejercicio { get; set; }
    }
}
