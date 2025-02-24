namespace GymAPI.DTOs
{
    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime FechaRegistro { get; set; }
        public bool EstaActivo { get; set; }
        
         public int Edad { get; set; }
        public double Peso { get; set; }
        public double Altura { get; set; }
    }
}
