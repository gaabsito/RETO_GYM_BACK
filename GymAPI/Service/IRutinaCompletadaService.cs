using GymAPI.Models;
using GymAPI.DTOs;

namespace GymAPI.Services
{
    public interface IRutinaCompletadaService
    {
        Task<List<RutinaCompletada>> GetAllAsync();
        Task<RutinaCompletada?> GetByIdAsync(int id);
        Task<List<RutinaCompletada>> GetByUsuarioIdAsync(int usuarioId);
        Task<List<RutinaCompletada>> GetByEntrenamientoIdAsync(int entrenamientoId);
        Task<List<RutinaCompletada>> GetByUsuarioIdAndEntrenamientoIdAsync(int usuarioId, int entrenamientoId);
        Task<int> AddAsync(RutinaCompletada rutinaCompletada);
        Task UpdateAsync(RutinaCompletada rutinaCompletada);
        Task DeleteAsync(int id);
        
        // Métodos para estadísticas
        Task<int> GetCountLastWeekAsync(int usuarioId);
        Task<int> GetCountLastMonthAsync(int usuarioId);
        Task<int> GetTotalCountAsync(int usuarioId);
        Task<(int EntrenamientoID, string Nombre, int Veces)> GetMostCompletedWorkoutAsync(int usuarioId);
        
        // Nuevos métodos
        Task<List<RutinaCompletada>> GetByUsuarioIdAndFechaAsync(int usuarioId, int month, int year);
        Task<Dictionary<DateTime, int>> GetCalendarDataAsync(int usuarioId, int month, int year);
        Task<RutinaEstadisticasDTO> GetEstadisticasUsuarioAsync(int usuarioId);
        
        // Nuevo método para días únicos entrenados esta semana
        Task<int> GetUniqueTrainingDaysThisWeekAsync(int usuarioId);
        
        // Método adicional para estadísticas semanales
        Task<Dictionary<int, int>> GetUniqueTrainingDaysLastWeeksAsync(int usuarioId, int numberOfWeeks);
    }
}