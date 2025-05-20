// GymAPI/DTOs/LogroDTO.cs
namespace GymAPI.DTOs
{
    public class LogroDTO
    {
        public int LogroID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public int Experiencia { get; set; }
        public string Categoria { get; set; } = "";
        public int ValorMeta { get; set; }
        public bool Secreto { get; set; }
    }

    public class UsuarioLogroDTO
    {
        public int LogroID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public int Experiencia { get; set; }
        public string Categoria { get; set; } = "";
        public bool Desbloqueado { get; set; }
        public DateTime? FechaDesbloqueo { get; set; }
        public int ProgresoActual { get; set; }
        public int ValorMeta { get; set; }
    }
}