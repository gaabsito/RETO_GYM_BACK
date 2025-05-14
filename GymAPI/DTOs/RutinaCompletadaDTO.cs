using System.ComponentModel.DataAnnotations;

namespace GymAPI.DTOs
{
    public class RutinaCompletadaDTO
    {
        public int RutinaCompletadaID { get; set; }
        public int UsuarioID { get; set; }
        public int EntrenamientoID { get; set; }
        public DateTime FechaCompletada { get; set; }
        public string? Notas { get; set; }
        public int? DuracionMinutos { get; set; }
        public int? CaloriasEstimadas { get; set; }
        public int? NivelEsfuerzoPercibido { get; set; }

        // Datos adicionales para la respuesta
        public string? NombreEntrenamiento { get; set; }
        public string? DificultadEntrenamiento { get; set; }
    }

    public class RutinaCompletadaCreateDTO
    {
        [Required]
        public int EntrenamientoID { get; set; }
        
        public DateTime? FechaCompletada { get; set; }
        
        [MaxLength(500, ErrorMessage = "Las notas no pueden superar los 500 caracteres")]
        public string? Notas { get; set; }
        
        [Range(1, 300, ErrorMessage = "La duración debe estar entre 1 y 300 minutos")]
        public int? DuracionMinutos { get; set; }
        
        [Range(1, 2000, ErrorMessage = "Las calorías deben estar entre 1 y 2000")]
        public int? CaloriasEstimadas { get; set; }
        
        [Range(1, 10, ErrorMessage = "El nivel de esfuerzo debe estar entre 1 y 10")]
        public int? NivelEsfuerzoPercibido { get; set; }
    }

    public class RutinaCompletadaUpdateDTO
    {
        public DateTime? FechaCompletada { get; set; }
        
        [MaxLength(500, ErrorMessage = "Las notas no pueden superar los 500 caracteres")]
        public string? Notas { get; set; }
        
        [Range(1, 300, ErrorMessage = "La duración debe estar entre 1 y 300 minutos")]
        public int? DuracionMinutos { get; set; }
        
        [Range(1, 2000, ErrorMessage = "Las calorías deben estar entre 1 y 2000")]
        public int? CaloriasEstimadas { get; set; }
        
        [Range(1, 10, ErrorMessage = "El nivel de esfuerzo debe estar entre 1 y 10")]
        public int? NivelEsfuerzoPercibido { get; set; }
    }

    public class ResumenRutinasDTO
    {
        public int TotalRutinasCompletadas { get; set; }
        public int RutinasUltimaSemana { get; set; }
        public int RutinasUltimoMes { get; set; }
        public double PromedioEsfuerzo { get; set; }
        public int CaloriasTotales { get; set; }
        public int MinutosTotales { get; set; }
        
        // Rutina más completada
        public int? EntrenamientoIDMasRepetido { get; set; }
        public string? NombreEntrenamientoMasRepetido { get; set; }
        public int? VecesCompletado { get; set; }
    }
}