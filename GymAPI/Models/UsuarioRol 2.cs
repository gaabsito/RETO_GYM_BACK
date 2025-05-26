namespace GymAPI.Models
{
    public class UsuarioRol
    {
        public int UsuarioRolID { get; set; }
        public int UsuarioID { get; set; }
        public int RolID { get; set; }
        public DateTime FechaAsignacion { get; set; } = DateTime.Now;
        public bool RolActual { get; set; } = true;
        
        // Navegación
        public Usuario? Usuario { get; set; }
        public Rol? Rol { get; set; }
    }
}