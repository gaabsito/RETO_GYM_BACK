namespace GymAPI.Models
{
    public class Comentario
    {
        public int ComentarioID { get; set; }
        public int EntrenamientoID { get; set; }
        public int? UsuarioID { get; set; }
        public string Contenido { get; set; } = "";
        public int Calificacion { get; set; }
        public DateTime FechaComentario { get; set; }
    }
}