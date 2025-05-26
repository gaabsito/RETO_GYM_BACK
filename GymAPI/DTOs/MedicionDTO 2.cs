namespace GymAPI.DTOs
{
    public class MedicionDTO
    {
        public int MedicionID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime Fecha { get; set; }
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

    public class MedicionCreateDTO
    {
        public int UsuarioID { get; set; }
        public DateTime? Fecha { get; set; }
        public float? Peso { get; set; }
        public float? Altura { get; set; }
        public float? PorcentajeGrasa { get; set; }
        public float? CircunferenciaBrazo { get; set; }
        public float? CircunferenciaPecho { get; set; }
        public float? CircunferenciaCintura { get; set; }
        public float? CircunferenciaMuslo { get; set; }
        public string? Notas { get; set; }
    }

    public class MedicionUpdateDTO
    {
        public DateTime? Fecha { get; set; }
        public float? Peso { get; set; }
        public float? Altura { get; set; }
        public float? PorcentajeGrasa { get; set; }
        public float? CircunferenciaBrazo { get; set; }
        public float? CircunferenciaPecho { get; set; }
        public float? CircunferenciaCintura { get; set; }
        public float? CircunferenciaMuslo { get; set; }
        public string? Notas { get; set; }
    }

    public class MedicionResumenDTO
    {
        public int Anio { get; set; }
        public int Mes { get; set; }
        public float? PesoPromedio { get; set; }
        public float? IMCPromedio { get; set; }
        public float? GrasaPromedio { get; set; }
        public float? CinturaPromedio { get; set; }
    }
}