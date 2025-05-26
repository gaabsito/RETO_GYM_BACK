namespace GymAPI.DTOs
{
    public class RutinaEstadisticasDTO
    {
        // Estadísticas generales
        public int TotalRutinasCompletadas { get; set; }
        public int RutinasUltimaSemana { get; set; }
        public int RutinasUltimoMes { get; set; }
        
        // Estadísticas de días únicos
        public int DiasUnicosEntrenadosEstaSemana { get; set; }
        public int SemanaActual { get; set; }
        
        // Promedios
        public double PromedioEsfuerzo { get; set; }
        public int CaloriasTotales { get; set; }
        public int MinutosTotales { get; set; }
        
        // Entrenamiento más frecuente
        public int? EntrenamientoIDMasRepetido { get; set; }
        public string? NombreEntrenamientoMasRepetido { get; set; }
        public int? VecesCompletado { get; set; }
        
        // Calendario de entrenos
        public List<FechaEntrenoDTO> DiasEntrenados { get; set; } = new List<FechaEntrenoDTO>();
        
        // Historial semanal
        public Dictionary<int, int> DiasUnicosPorSemana { get; set; } = new Dictionary<int, int>();
    }

    public class FechaEntrenoDTO
    {
        public DateTime Fecha { get; set; }
        public int EntrenamientoID { get; set; }
        public string? NombreEntrenamiento { get; set; }
    }
}