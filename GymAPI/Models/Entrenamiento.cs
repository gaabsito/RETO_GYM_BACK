namespace GymAPI.Models
{
    public class Entrenamiento
    {
        public int EntrenamientoID { get; set; }
        public string Titulo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int DuracionMinutos { get; set; }

        public string Dificultad { get; set; }  // Ahora es un string simple
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public bool Publico { get; set; } = true;

        public int? AutorID { get; set; }

        public Usuario? Autor { get; set; }
    }
}
