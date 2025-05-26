// GymAPI/Models/UsuarioLogro.cs
namespace GymAPI.Models
{
    public class UsuarioLogro
    {
        public int UsuarioLogroID { get; set; }
        public int UsuarioID { get; set; }
        public int LogroID { get; set; }
        public DateTime FechaDesbloqueo { get; set; } = DateTime.Now;
        public int ProgresoActual { get; set; } = 0;
        public bool Desbloqueado { get; set; } = false;
        
        // Propiedades de navegación
        public Usuario? Usuario { get; set; }
        public Logro? Logro { get; set; }
    }
}