namespace GymAPI.Models
{
    public class MedicionResumen
    {
        public int UsuarioID { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
        public float? PesoPromedio { get; set; }
        public float? IMCPromedio { get; set; }
        public float? GrasaPromedio { get; set; }
        public float? CinturaPromedio { get; set; }
    }
}