namespace GymAPI.Models
{
    public class Medicion
    {
        public int MedicionID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        
        // Medidas corporales
        public float? Peso { get; set; }
        public float? Altura { get; set; }
        public float? IMC { get; set; }
        public float? PorcentajeGrasa { get; set; }
        public float? CircunferenciaBrazo { get; set; }
        public float? CircunferenciaPecho { get; set; }
        public float? CircunferenciaCintura { get; set; }
        public float? CircunferenciaMuslo { get; set; }
        public string? Notas { get; set; }
    }
}