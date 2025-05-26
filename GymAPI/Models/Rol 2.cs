namespace GymAPI.Models
{
    public class Rol
    {
        public int RolID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public int DiasPorSemanaMinimo { get; set; }
        public int DiasPorSemanaMaximo { get; set; }
    }
}