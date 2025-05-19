namespace GymAPI.DTOs
{
    public class RolDTO
    {
        public int RolID { get; set; }
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public int DiasPorSemanaMinimo { get; set; }
        public int DiasPorSemanaMaximo { get; set; }
    }

    public class UsuarioRolDTO
    {
        public int UsuarioID { get; set; }
        public int RolID { get; set; }
        public string NombreRol { get; set; } = "";
        public string Color { get; set; } = "";
        public string Icono { get; set; } = "";
        public DateTime FechaAsignacion { get; set; }
        public int DiasEntrenadosSemanales { get; set; }  // Renombrado para claridad
        public int DiasParaSiguienteRol { get; set; }
        public double ProgresoSiguienteRol { get; set; }
        public int SemanaActual { get; set; } // Nueva propiedad para identificar la semana actual
    }
}