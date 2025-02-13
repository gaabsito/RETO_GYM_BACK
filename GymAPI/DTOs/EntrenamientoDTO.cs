namespace GymAPI.DTOs
{
    public class EntrenamientoDTO
    {
        public int EntrenamientoID { get; set; }
        public string Titulo { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int DuracionMinutos { get; set; }
        public string Dificultad { get; set; } = "";
        public string? ImagenURL { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Publico { get; set; }
        public int? AutorID { get; set; }
    }
}
